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

        List<IAdaptiveAgent> CreatePopulation() 
        {
            List<IAdaptiveAgent> result = new List<IAdaptiveAgent>();

            AgentBuilder agentBuilder = new AgentBuilder();
            result.Add(agentBuilder.SetInitialPosition(1, 1).SetSpawnRound(0).AddActionNode(ActionType.Forward).BuildAgent());

            return result;
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
