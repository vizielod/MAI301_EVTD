using System;
using System.Collections.Generic;
using System.Linq;

namespace Simulator.state
{
    class State : IState
    {
        public TileType[,] MapLayout { get; }
        public IEnumerable<IAgent> Agents => _agents.Select(p => p.Key);

        private IDictionary<IAgent, IStateObject> _agents;
        private IEnumerable<Event> _events;

        public IEnumerable<IAgent> GetAgentsAt(int x, int y)
        {
            return _agents.Where(p => p.Value.GridLocation == (x, y)).Select(p => p.Key);            
        }

        public (int x, int y) PositionOf(IAgent agent)
        {
            throw new NotImplementedException();
        }

        internal void AddAgent(IAgent agent)
        {
            throw new NotImplementedException();
        }
    }
}
