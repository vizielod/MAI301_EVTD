using System.Collections.Generic;
using System.Linq;

namespace Simulator.state
{
    class Round
    {
        private IEnumerable<Event> events;

        public Round(IEnumerable<Event> events)
        {
            this.events = events;
        }

        public void ApplyAll(IGame game)
        {
            events.AsParallel().ForAll((evnt) => { evnt.Action.Apply(game.GetStateObject(evnt.Agent)); });
        }

        public void UndoAll(IGame game)
        {
            events.AsParallel().ForAll((evnt) => { evnt.Action.Undo(game.GetStateObject(evnt.Agent)); });
        }
    }
}
