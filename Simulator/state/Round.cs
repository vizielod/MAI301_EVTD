using System.Collections.Generic;
using System.Linq;

namespace Simulator.state
{
    class Round
    {
        private IList<Event> events;

        public Round(IList<Event> events)
        {
            this.events = events;
        }

        public void ApplyAll(IGame game)
        {
            events.AsParallel().ForAll((evnt) => 
            { 
                evnt.Action.Apply(game.GetStateObject(evnt.Agent)); 
            });
        }

        public void UndoAll(IGame game)
        {
            events.AsParallel().ForAll((evnt) => 
            { 
                evnt.Action.Undo(game.GetStateObject(evnt.Agent)); 
            });
        }

        private void ScoreAll(IGame game)
        {
            // Calculate score
            var goals = 0;
            var enemies = 0;
            foreach (var evnt in events)
            {
                IStateObject stateObj = game.GetStateObject(evnt.Agent);
                if (stateObj.GoalReached) goals++;
                
            }

            // Apply score for every event
        }
    }
}
