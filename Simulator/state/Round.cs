using System;
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
                var state = game.GetStateObject(evnt.Agent);
                var originalPosition = state.GridLocation;
                evnt.Action.Apply(state);
                state.HasMoved = originalPosition != state.GridLocation;
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

        public void CalculateScores(IGame game)
        {
            var goals = game.CountEnemiesSuccess();
            var activeEnemies = game.CountActiveEnemies();
            var totalEnemies = game.CountEnemies();
            
            events.AsParallel().ForAll(e => 
            {
                var sObj = game.GetStateObject(e.Agent);
                if (e.Agent.IsEnemy)
                {
                    float degradation = 1 - ((roundNumber - e.Agent.SpawnRound) * (1 / game.RoundLimit));

                    if (e.Agent.IsActive && sObj.Spawned)
                    {
                        float successfulEnemies = goals + activeEnemies;
                        float socialScore = (successfulEnemies + 1) / (totalEnemies + 1);
                        float healthRatio = e.Agent.HealthRatio;
                        float idlePenalty = sObj.HasMoved ? 1 : 0.5f;
                        e.Reward = game.GetProgression(e.Agent);
                    }
                    else if (sObj.GoalReached)
                    {
                        e.Reward = 1;
                    }

                    //e.Reward *= degradation;
                }
            });
        }

        public IEnumerable<(IAgent agent, float score)> GetScores()
        {
            foreach (var evnt in events)
                yield return (evnt.Agent, evnt.Reward);
        }
    }
}
