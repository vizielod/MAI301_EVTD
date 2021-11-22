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

        public void ScoreAll(IGame game)
        {
            var goals = 0;
            var activeEnemies = 0;
            foreach (var evnt in events)
            {
                IStateObject stateObj = game.GetStateObject(evnt.Agent);
                if (stateObj.GoalReached) goals++;
                if (stateObj.IsEnemy && evnt.Agent.IsActive && stateObj.IsActive) activeEnemies++;
            }

            events.AsParallel().ForAll(e => 
            {
                if (e.Agent.IsActive)
                    e.Reward = (goals + activeEnemies + 1) / (game.CountEnemies() + 1);
            });
        }
    }
}
