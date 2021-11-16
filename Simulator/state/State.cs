using System.Collections.Generic;
using System.Linq;

namespace Simulator.state
{
    class State : IState
    {
        public IMapLayout MapLayout { get; }
        public IEnumerable<IAgent> Agents => agents.Select(p => p.Key);

        private IDictionary<IAgent, StateObject> agents;
        private List<Event> events;
        internal IEnumerable<Event> Events => events;

        public State(IMapLayout map)
        {
            MapLayout = map;
            agents = new Dictionary<IAgent, StateObject>();
        }

        public void AddAgent(IAgent agent, (int x, int y) gridLocation, AgentType type)
        {
            agents.Add(agent, new StateObject(gridLocation) { 
                IsActive = true, 
                Type = type
            });
        }

        public IEnumerable<IAgent> GetAgentsAt(int x, int y)
        {
            return agents.Where(p => p.Value.GridLocation == (x, y)).Select(p => p.Key);            
        }

        public (int x, int y) PositionOf(IAgent agent)
        {
            return agents[agent].GridLocation;
        }

        public IActionGenerator GetLegalActionGenerator(IAgent agent)
        {
            return new LegalMoveGenerator(MapLayout, agents[agent]); // TODO: Fix Control Freak anti pattern
        }

        public IAgent GetClosestEnemy(IAgent agent)
        {
            throw new System.NotImplementedException();
        }
    }
}
