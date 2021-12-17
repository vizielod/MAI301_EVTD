using BehaviorTree;
using BehaviorTree.Agents;
using BehaviorTree.NodeBase;
using Simulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Evolution
{
    public class EvolutionConfiguration
    {
        private float eliteRate = 0.02f;
        private float mutationRate = 0.5f;
        private float roulettRate = 0.5f;

        public float MutationRate { 
            get { return mutationRate; }
            set { mutationRate = value.Clamp(0, 1); }
        }
        public int PopulationSize { get; set; }
        public int NumberOfGenerations { get; set; }
        public float EliteRate {
            get { return eliteRate; }
            set { eliteRate = value.Clamp(0, 1); }
        }
        public float RoulettRate
        {
            get { return roulettRate; }
            set { roulettRate = value.Clamp(0, 1); }
        }

        internal int Elites => (int)Math.Floor(PopulationSize * EliteRate);
        internal int Roulett => (int)Math.Floor(PopulationSize * RoulettRate);
    }

    public class Evolutionary
    {
        public IStateSequence NewestSimulation { get; private set; }
        public int CurrentGeneration { get; private set; }

        private readonly EvolutionConfiguration configuration;
        private SimulatorFactory factory;
        private Random rand = new Random();

        private IAgent zinger;
        private float zingerScore = 0;

        public Evolutionary(EvolutionConfiguration configuration)
        {
            CurrentGeneration = 0;
            NewestSimulation = null;
            factory = new SimulatorFactory();
            this.configuration = configuration;
        }

        IEnumerable<IEnemyAgent> CreatePopulation(int size) 
        {
            for (int i = 0; i < size; i++)
            {
                var agentBuilder = new AgentBuilder()
                    .SetInitialPosition(1, 1)
                    .SetSpawnRound(i)
                    .AddNodesToRoot(RandomSelect.Random<CompositeType>(), RandomSelect.Random<ConditionType>(), RandomSelect.Random<ActionType>());

                // Random complexity
                int chance;
                while ((chance = rand.Next(20)) != 0)
                {
                    if (chance < 5)
                    {
                        agentBuilder.AddCompositeNode(RandomSelect.Random<CompositeType>());
                    }
                    else if (chance < 10)
                    {
                        agentBuilder.AddActionNode(RandomSelect.Random<ActionType>());
                    }
                    else if (chance < 15)
                    {
                        agentBuilder.AddConditionNode(RandomSelect.Random<ConditionType>());
                    }
                    else
                    {
                        agentBuilder.AddNodesToRoot(RandomSelect.Random<CompositeType>(), RandomSelect.Random<ConditionType>(), RandomSelect.Random<ActionType>());
                    }
                }

                yield return agentBuilder.BuildAgent();
            }
        }

        IReadOnlyDictionary<IAgent, float> SortIndividualInPopulation(IReadOnlyDictionary<IAgent, float> scores)
        {
            var result = scores.Where(a => a.Key.IsEnemy).ToList();

            result.Sort((pair1, pair2) =>  pair1.Value.CompareTo(pair2.Value));
            result.Reverse();
            return result.ToDictionary(s => s.Key, s=> s.Value);
        }

        IEnumerable<IAgent> ElitistSelection(IReadOnlyDictionary<IAgent, float> scores, int size)
        {
            var result = scores.Keys.ToList();
            return result.Take(size).Cast<IAdaptiveEnemy>().Select(e=>e.Clone());
        }

        IEnumerable<IAgent> RoulettSelection(IReadOnlyDictionary<IAgent, float> scores, int size)
        {
            for (int i = 0; i < size; i++)
            {
                float total = scores.Values.Sum();
                float chance = (float)rand.NextDouble() * total;
                foreach (var enemy in scores)
                {
                    if ((chance -= enemy.Value) < 0)
                    {
                        yield return ((IAdaptiveEnemy)enemy.Key).Clone();
                        break;
                    }
                }
            }
        }

        IReadOnlyDictionary<IAgent, float> RunSimulation(IMapLayout map, IEnumerable<IAgent> enemies, IEnumerable<IAgent> turrets)
        {
            IStateSequence stateSequence = factory.CreateSimulator(map, enemies, turrets);

            while (!stateSequence.IsGameOver)
            {
                stateSequence.StepForward();
            }

            IReadOnlyDictionary<IAgent, float> scores = stateSequence.GetScores();

            scores = SortIndividualInPopulation(scores);

            stateSequence.ReWind();

            NewestSimulation = stateSequence;

            return scores;
        }

        public IEnumerable<float> RunEvolution(IMapLayout map, IEnumerable<IAgent> turrets)
        {
            IEnumerable<IAgent> enemies = CreatePopulation(configuration.PopulationSize).Cast<IAgent>();

            IReadOnlyDictionary<IAgent, float> scores = RunSimulation(map, enemies, turrets);
            yield return scores.Sum(s => s.Value);

            // Set termination condition here
            for (int i = 0; i < configuration.NumberOfGenerations; i++)
            {
                CurrentGeneration = i + 1;

                // Randomly select candidates, fitness score as probability
                var population = RoulettSelection(scores, configuration.Roulett);
                
                // Breed the selected candidates, one child per 2 parents, implies mutation
                population = BreedPopulation(population);

                // Find out if there is a outstanding enemy in the population
                var zinger = scores.First();
                if (zinger.Value > zingerScore)
                    this.zinger = ((IAdaptiveEnemy)zinger.Key).Clone();

                population = population.Concat(ToList(this.zinger));

                // Take elites, no breeding or altering
                population = population.Concat(ElitistSelection(scores, configuration.Elites));

                // Make sure the population is the correct size
                var remaining = configuration.PopulationSize - population.Count();
                if (remaining > 0) // If there is room left, then fill it with randomly generated enemies
                    population = population.Concat(CreatePopulation(remaining));
                else if (remaining < 0) // if the population is too big, then cut it down
                    population = population.Take(configuration.PopulationSize);

                scores = RunSimulation(map, population, turrets);
                yield return scores.Sum(s => s.Value);
            }
        }

        private IEnumerable<T> ToList<T>(params T[] list) => list;

        private IEnumerable<IAgent> BreedPopulation(IEnumerable<IAgent> population)
        {
            int i = 0;
            AgentBuilder mom = null;

            var factory = new NodeCrosserFactory();

            foreach (var enemy in population.Cast<IAdaptiveEnemy>())
            {
                AgentBuilder builder = enemy.ReverseEngineer();
                if (++i % 2 == 0)
                {
                    factory.Create(mom, builder).Cross();
                    if (rand.NextDouble() < configuration.MutationRate)
                        builder.Mutate();
                    if (rand.NextDouble() < configuration.MutationRate)
                        mom.Mutate();

                    yield return mom.BuildAgent();
                    yield return builder.BuildAgent();
                    mom = null;
                }
                else
                {
                    mom = builder;
                }
            }

            if (mom != null)
            {
                if (rand.NextDouble() < configuration.MutationRate)
                    mom.Mutate();
                yield return mom.BuildAgent();
            }
        }

        public async Task RunEvolutionAsync(IMapLayout map, IEnumerable<IAgent> turrets, Action<float> score_cb)
        {
            foreach (float item in RunEvolution(map, turrets))
            {
                score_cb.Invoke(item);
                await Task.Yield();
            }
        }
    }
}
