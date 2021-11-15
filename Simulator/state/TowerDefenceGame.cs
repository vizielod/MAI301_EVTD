using System.Collections.Generic;
using System.Linq;

namespace Simulator.state
{
    class TowerDefenceGame : IGame
    {
        private readonly IMapLayout map;
        private readonly Dictionary<IAgent, StateObject> agents;

        public TowerDefenceGame(IMapLayout map, IEnumerable<IAgent> agents, IEnumerable<IAgent> towers)
        {
            this.map = map;
            this.agents = new Dictionary<IAgent, StateObject>();
            foreach (var agent in agents)
            {
                this.agents.Add(agent, new StateObject(agent.InitialPosition)
                { 
                    IsActive = false, 
                    Type = AgentType.Enemy 
                });
            }
            foreach (var tower in towers)
            {
                this.agents.Add(tower, new StateObject(tower.InitialPosition)
                {
                    IsActive = true,
                    Type = AgentType.Tower
                });
            }
        }

        public IEnumerable<IAgent> Agents => agents.Where(a => a.Value.IsActive).Select(a => a.Key);

        public IState GenerateState()
        {
            var state = new State(map);
            foreach (var agent in agents)
                state.AddAgent(agent.Key, agent.Value.GridLocation, agent.Value.Type);

            return state;
        }

        public IStateObject GetStateObject(IAgent agent)
        {
            return agents.First(a => a.Key == agent).Value;
        }
    }
}
