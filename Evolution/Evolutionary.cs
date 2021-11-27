using BehaviorTree.Agents;
using Simulator;
using System;
using System.Collections.Generic;


namespace Evolution
{
    class Evolutionary
    {
        int populationSize;

        public Evolutionary(int populationSize)
        {
            this.populationSize = populationSize;
        }

        List<IAdaptiveAgent> CreatePopulation() 
        {
            List<IAdaptiveAgent> result = new List<IAdaptiveAgent>();

            AgentBuilder agentBuilder = new AgentBuilder();
            result.Add(agentBuilder.SetInitialPosition(1, 1).SetSpawnRound(0).AddActionNode(ActionType.Forward).BuildAgent());

            return result;
        }

        public IEnumerable<IStateSequence> RunEvolution(IMapLayout map, List<IAgent> turrets)
        {
            return null;
        }
    }
}
