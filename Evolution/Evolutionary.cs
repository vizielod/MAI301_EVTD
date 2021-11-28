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

        public Evolutionary(int populationSize)
        {
            this.populationSize = populationSize;
            factory = new SimulatorFactory();
        }

        IEnumerable<IAdaptiveAgent> CreatePopulation() 
        {
            //List<IAdaptiveAgent> result = new List<IAdaptiveAgent>();

            for (int i = 0; i < populationSize; i++)
            {
                AgentBuilder agentBuilder = new AgentBuilder();
                yield return agentBuilder.SetInitialPosition(1, 1).SetSpawnRound(i).AddActionNode(ActionType.Forward).BuildAgent();
            }
           
        }

        public IEnumerable<IStateSequence> RunEvolution(IMapLayout map, IEnumerable<IAgent> turrets)
        {
            IEnumerable<IAgent> enemies = CreatePopulation().Cast<IAgent>();

            IStateSequence stateSequence = factory.CreateSimulator(map, enemies, turrets);

            while (!stateSequence.IsGameOver)
            {
                stateSequence.StepForward();
            }

            yield return stateSequence;
        }
    }
}
