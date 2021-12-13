using BehaviorTree.Agents;
using Simulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Evolution
{
    public class Evolutionary
    {
        public IStateSequence NewestSimulation { get; private set; }
        public int CurrentGeneration { get; private set; }

        int populationSize;
        int generationLength;
        SimulatorFactory factory;
        Random rand;

        public Evolutionary(int populationSize, int generationLength)
        {
            CurrentGeneration = 0;
            NewestSimulation = null;
            this.populationSize = populationSize;
            this.generationLength = generationLength;
            factory = new SimulatorFactory();
            rand = new Random();
        }

        T Random<T>()
        {
            Array array = Enum.GetValues(typeof(T));
            
            return (T)array.GetValue(rand.Next(array.Length));
        }

        IEnumerable<IEnemyAgent> CreatePopulation(int size) 
        {
            for (int i = 0; i < size; i++)
            {
                yield return new AgentBuilder()
                    .SetInitialPosition(1, 1)
                    .SetSpawnRound(i)
                    .AddRootNodes(Random<CompositeType>(), Random<ConditionType>(), Random<ActionType>())
                    .AddRootNodes(Random<CompositeType>(), Random<ConditionType>(), Random<ActionType>())
                    .AddRootNodes(Random<CompositeType>(), Random<ConditionType>(), Random<ActionType>())
                    .AddRootNodes(Random<CompositeType>(), Random<ConditionType>(), Random<ActionType>())
                    .BuildAgent();
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
            return result.Take(size);
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
            IEnumerable<IAgent> enemies = CreatePopulation(populationSize).Cast<IAgent>();

            IReadOnlyDictionary<IAgent, float> scores = RunSimulation(map, enemies, turrets);
            yield return scores.Sum(s => s.Value);

            // Set termination condition here
            for (int i = 0; i < generationLength; i++)
            {
                CurrentGeneration = i + 1;
                var population = ElitistSelection(scores, populationSize/4);
                population = population.Concat(ClonePopulation(population));
                population = population.Concat(CreatePopulation(populationSize - population.Count()));
                scores = RunSimulation(map, population, turrets);
                yield return scores.Sum(s => s.Value);
            }
        }

        private IEnumerable<IAgent> ClonePopulation(IEnumerable<IAgent> population)
        {
            foreach (IEnemyAgent enemy in population.Cast<IEnemyAgent>())
                yield return enemy.Clone();
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
