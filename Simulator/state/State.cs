using System.Collections.Generic;
using System.Linq;

namespace Simulator.state
{
    class State : IState
    {
        public IMapLayout MapLayout { get; set; }
        public IEnumerable<IAgent> Agents => _agents.Select(p => p.Key);

        private IDictionary<IAgent, StateObject> _agents;
        private IEnumerable<Event> _events;

        public void Apply()
        {
            foreach (var evnt in _events)
            {
                evnt.Action.Apply();
            }
        }

        public IEnumerable<IAgent> GetAgentsAt(int x, int y)
        {
            return _agents.Where(p => p.Value.GridLocation == (x, y)).Select(p => p.Key);            
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
