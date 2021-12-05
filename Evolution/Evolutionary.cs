using BehaviorTree.Agents;
using Simulator;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Evolution
{
    public class Evolutionary
    {
        int populationSize;
        SimulatorFactory factory;
        Random rand;

        public Evolutionary(int populationSize)
        {
            this.populationSize = populationSize;
            factory = new SimulatorFactory();
            rand = new Random();
        }

        T Random<T>()
        {
            Array array = Enum.GetValues(typeof(T));
            
            return (T)array.GetValue(rand.Next(array.Length));
        }

        IEnumerable<IEnemyAgent> CreatePopulation() 
        {
            //List<IAdaptiveAgent> result = new List<IAdaptiveAgent>();

            for (int i = 0; i < populationSize; i++)
            {
                AgentBuilder agentBuilder = new AgentBuilder();

                yield return agentBuilder.SetInitialPosition(1, 1).SetSpawnRound(i).AddRootNodes(Random<CompositeType>(), Random<ConditionType>(), Random<ActionType>()).BuildAgent();
            }
           
        }

        IDictionary<IAgent, float> SortIndividualInPopulation(IDictionary<IAgent, float> scores)
        {
            var result = scores.ToList();

            result.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));

            return result.ToDictionary(s => s.Key, s=> s.Value);
        }

        IDictionary<IAgent, float> ElitistSelection(IDictionary<IAgent, float> scores)
        {
            return null;
        }

        public IEnumerable<IStateSequence> RunEvolution(IMapLayout map, IEnumerable<IAgent> turrets)
        {
            IEnumerable<IAgent> enemies = CreatePopulation().Cast<IAgent>();
            

            IStateSequence stateSequence = factory.CreateSimulator(map, enemies, turrets);

            while (!stateSequence.IsGameOver)
            {
                stateSequence.StepForward();
            }

            IDictionary<IAgent, float> scores = stateSequence.GetScores();

            scores = SortIndividualInPopulation(scores);

            // Set termination condition here
            while (true)
            {

            }


            stateSequence.ReWind();

            yield return stateSequence;
        }
    }
}
