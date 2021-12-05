using System.Collections.Generic;
using System.Linq;

namespace Simulator.state
{
    class Round
    {
        private IList<Event> events;
        private readonly int roundNumber;

        public Round(IList<Event> events, int roundNumber)
        {
            this.events = events;
            this.roundNumber = roundNumber;
        }

        public void ApplyAll(IGame game)
        {
            game.NewRound();
            events.AsParallel().ForAll((evnt) => 
            { 
                evnt.Action.Apply(game.GetStateObject(evnt.Agent)); 
            });
        }

        public void UndoAll(IGame game)
        {
            game.NewRound();
            events.AsParallel().ForAll((evnt) => 
            { 
                evnt.Action.Undo(game.GetStateObject(evnt.Agent)); 
            });
        }

        public void ScoreAll(IGame game)
        {
            var goals = game.CountEnemiesSuccess();
            var activeEnemies = game.CountActiveEnemies();
            var enemies = game.CountEnemies();
            
            events.AsParallel().ForAll(e => 
            {
                var sObj = game.GetStateObject(e.Agent);
                if (sObj.IsEnemy)
                {
                    if (e.Agent.IsActive && sObj.IsActive)
                        e.Reward = (goals + activeEnemies + 1) / (enemies + 1) * (1 / (roundNumber - e.Agent.SpawnRound + 1));
                    else if (sObj.GoalReached)
                        e.Reward = 1;
                }
            });
        }
    }
}
