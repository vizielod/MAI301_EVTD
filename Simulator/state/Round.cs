using System.Collections.Generic;
using System.Linq;

namespace Simulator.state
{
    class Round
    {
        private IEnumerable<Event> _events;

        public Round(IEnumerable<Event> events)
        {
            _events = events;
        }

        public void ApplyAll()
        {
            _events.AsParallel().ForAll((evnt) => { evnt.Action.Apply(); });
        }

        public void UndoAll()
        {
            _events.AsParallel().ForAll((evnt) => { evnt.Action.Undo(); });
        }

    }
}
