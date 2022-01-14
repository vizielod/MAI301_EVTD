using BehaviorTree;
using BehaviorTree.Agents;
using BehaviorTree.NodeBase;
using Simulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Evolution
{
    public class EvolutionConfiguration
    {
        private float eliteRate = 0.02f;
        private float mutationRate = 0.5f;
        private float roulettRate = 0.5f;
        private float compositeNodeRate = 0.3f;
        private float actionNodeRate = 0.3f;
        private float breathVsDepth = 0.3f;

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
        public float LeafVsCompositeNodes
        {
            get { return compositeNodeRate; }
            set { compositeNodeRate = value.Clamp(0, 1); }
        }
        public float ConditionalVsActionNodes
        {
            get { return actionNodeRate; }
            set { actionNodeRate = value.Clamp(0, 1); }
        }
        public float BreathVsDepth
        {
            get { return breathVsDepth; }
            set { breathVsDepth = value.Clamp(0, 1); }
        }

        public bool UseZinger { get; set; }
        public int TreeGeneratorIterations { get; set; }
        public bool CrossComposites { get; set; }
        internal int Elites => (int)Math.Floor(PopulationSize * EliteRate);
        internal int Roulett => (int)Math.Floor(PopulationSize * RoulettRate);
    }

    public class Evolutionary
    {
        public IStateSequence NewestSimulation { get; private set; }
        public int CurrentGeneration { get; private set; }
        public bool AsyncIsRunning { get; internal set; }
        public TimeSpan? TimeDuration { get; internal set; }

        private readonly EvolutionConfiguration configuration;
        private readonly TowerDefenceSimulatorFactory factory;
        private Random rand = new Random();

        private IAgent zinger;
        private float zingerScore = 0;

        public Evolutionary(EvolutionConfiguration configuration, TowerDefenceSimulatorFactory factory)
        {
            CurrentGeneration = 0;
            NewestSimulation = null;
            this.configuration = configuration;
            this.factory = factory;
        }

        void AddNodes(AgentBuilder agentBuilder)
        {
            if (rand.NextDouble() < configuration.ConditionalVsActionNodes)
            {
                agentBuilder.AddActionNode(RandomSelect.Random<ActionType>());
            }
            else
            {
                agentBuilder.AddConditionNode(RandomSelect.Random<ConditionType>());
            }
        }

        IEnumerable<IEnemyAgent> CreatePopulation(int size) 
        {
            for (int i = 0; i < size; i++)
            {
                var agentBuilder = new AgentBuilder(RandomSelect.Random<CompositeType>())
                    .SetInitialPosition(1, 1)
                    .SetSpawnRound(i);

                // Random complexity
                int iterationsLeft = configuration.TreeGeneratorIterations;
                for(int n = 0; n < iterationsLeft; n++)
                {
                    if (rand.NextDouble() < configuration.LeafVsCompositeNodes)
                    {
                        if (rand.NextDouble() < configuration.BreathVsDepth)
                            agentBuilder.AddAlternateComposite();
                        else
                            agentBuilder.AddAlternateCompositeToRoot();
                    }
                    AddNodes(agentBuilder);
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
            var elites = result.Take(size).Cast<IAdaptiveEnemy>().Select(e=>e.Clone());
            return elites;
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

        IReadOnlyDictionary<IAgent, float> RunSimulation(IEnumerable<IAgent> enemies)
        {
            IStateSequence stateSequence = factory.Create(enemies);

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

        public IEnumerable<float> RunEvolution()
        {
            return RunEvolution(CreatePopulation(configuration.PopulationSize).Cast<IAdaptiveEnemy>());
        }

        public IEnumerable<float> RunEvolution(IEnumerable<IAdaptiveEnemy> enemies)
        {
            IReadOnlyDictionary<IAgent, float> scores = RunSimulation(enemies);
            yield return scores.Sum(s => s.Value);

            // Set termination condition here
            for (int i = 0; i < configuration.NumberOfGenerations; i++)
            {
                // Terminating if the enemies won
                if (NewestSimulation.Winner == Alliances.Enemies)
                {
                    break;
                }

                CurrentGeneration = i + 1;

                // Randomly select candidates, fitness score as probability
                var population = RoulettSelection(scores, configuration.Roulett);
                
                // Breed the selected candidates, one child per 2 parents, implies mutation
                population = BreedPopulation(population);

                if (configuration.UseZinger)
                {
                    // Find out if there is a outstanding enemy in the population
                    var zinger = scores.First();
                    if (zinger.Value > zingerScore)
                    {
                        this.zinger = ((IAdaptiveEnemy)zinger.Key).Clone();
                        zingerScore = zinger.Value;
                    }

                    population = population.Concat(ToList(this.zinger));
                }

                // Take elites, no breeding or altering
                population = population.Concat(ElitistSelection(scores, configuration.Elites));

                // Make sure the population is the correct size
                var remaining = configuration.PopulationSize - population.Count();
                if (remaining > 0) // If there is room left, then fill it with randomly generated enemies
                    population = population.Concat(CreatePopulation(remaining));
                else if (remaining < 0) // if the population is too big, then cut it down
                    population = population.Take(configuration.PopulationSize);

                scores = RunSimulation(population);
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
                    if (configuration.CrossComposites)
                        factory.Create(mom, builder).Cross();
                    else
                        factory.CreateLeafNode(mom, builder).Cross();

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

        public async Task RunEvolutionAsync(Action<GenerationInfo> iterationComplete, Action<TimeSpan> evolutionComplete = null)
        {
            Thread thread = new Thread(new ThreadStart(new EvolutionThread(this, iterationComplete).Run))
            {
                IsBackground = true
            };
            thread.Start();
            
            AsyncIsRunning = true;
            DateTime startTime = DateTime.Now;
            while (thread.IsAlive)
                await Task.Yield();
            TimeDuration = DateTime.Now - startTime;
            AsyncIsRunning = false;
            
            if (evolutionComplete != null)
                evolutionComplete.Invoke(TimeDuration.Value);
        }
    }

    class EvolutionThread
    {
        private readonly Evolutionary evolution;
        private readonly IMapLayout map;
        private readonly IEnumerable<IAgent> turrets;
        private readonly Action<GenerationInfo> callback;

        public EvolutionThread(Evolutionary evolution, Action<GenerationInfo> callback)
        {
            this.evolution = evolution;
            this.callback = callback;
        }

        public void Run()
        {
            foreach (float score in evolution.RunEvolution())
            {
                callback.Invoke(new GenerationInfo(evolution.CurrentGeneration, score, evolution.NewestSimulation));
            }
        }
    }

}
