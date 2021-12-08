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
            //List<IAdaptiveAgent> result = new List<IAdaptiveAgent>();

            for (int i = 0; i < size; i++)
            {
                AgentBuilder agentBuilder = new AgentBuilder();

                yield return agentBuilder.SetInitialPosition(1, 1).SetSpawnRound(i).AddRootNodes(Random<CompositeType>(), Random<ConditionType>(), Random<ActionType>()).BuildAgent();
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
            result.Take(size);

            return result;
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
                IEnumerable<IAgent> population = ElitistSelection(scores, populationSize/2);
                population.Union( CreatePopulation(populationSize / 2));
                scores = RunSimulation(map, population, turrets);
                yield return scores.Sum(s => s.Value);
            }
        }

        public async void RunEvolutionAsync(IMapLayout map, IEnumerable<IAgent> turrets, Action<float> score_cb)
        {
            foreach (float item in RunEvolution(map, turrets))
            {
                score_cb.Invoke(item);
            }
        }
    }
}
