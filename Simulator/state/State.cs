using System;
using System.Collections.Generic;
using System.Linq;

namespace Simulator.state
{
    class State : IState
    {
        public IMapLayout MapLayout { get; }
        public IEnumerable<IAgent> Agents => _agents.Select(p => p.Key);

        private IDictionary<IAgent, StateObject> _agents;
        private List<Event> _events;
        internal IEnumerable<Event> Events => _events;

        public IEnumerable<IAgent> GetAgentsAt(int x, int y)
        {
            return _agents.Where(p => p.Value.GridLocation == (x, y)).Select(p => p.Key);            
        }
        public State(IMapLayout map)
        {
            MapLayout = map;
            _events = new List<Event>();
            _agents = new Dictionary<IAgent, StateObject>();
        }

        internal State Clone()
        {
            var state = new State(MapLayout);
            foreach (var agent in _agents)
            {
                (int x, int y) = agent.Value.GridLocation;
                state.AddAgent(agent.Key, x, y);
            }
            return state;
        }

        internal void AddEvent(Event e)
        {
            _events.Add(e);
        }

        public (int x, int y) PositionOf(IAgent agent)
        {
            return _agents[agent].GridLocation;
        }

        internal void AddAgent(IAgent agent, int x, int y)
        {
            _agents.Add(agent, new StateObject { GridLocation = (x, y) });
        }

        public IActionGenerator GetLegalActionGenerator(IAgent agent)
        {
            return new LegalMoveGenerator(MapLayout, _agents[agent]); // TODO: Fix Control Freak anti pattern
        }
    }
}
