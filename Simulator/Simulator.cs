using System.Collections.Generic;
using System.Linq;
using Simulator.gamespecific;
using Simulator.state;

namespace Simulator
{
    public class Simulator : IStateSequence
    {
        private readonly List<Round> rounds;
        private readonly IGame game;
        private readonly IWinCondition winCondition;
        IDictionary<IAgent, float> scoreboard;
        int roundIndex;

        public IEnumerable<IAgent> AllAgents => game.AllAgents;

        public IEnumerable<IAgent> AllEnemyAgents => game.AllEnemyAgents;

        public bool IsGameOver => winCondition.GetWinner(roundIndex).HasValue;

        internal Simulator(IGame game)
        {
            this.game = game;
            rounds = new List<Round>();
            roundIndex = -1;
            scoreboard = new Dictionary<IAgent, float>();
            winCondition = new WinConditionChain(new EnemiesDefeatedWinCondition(game), new WinConditionChain(new EnemiesGoalReachedWinCondition(game), new TimeoutWinCondition(200)));
        }

        public void StepForward()
        {
            if (IsGameOver)
            {
                return;
            }

            roundIndex++;
            game.SpawnAgents(roundIndex);

            

            var newRound = roundIndex >= rounds.Count;
            
            if (newRound)
            {
                IState state = game.GenerateState();
                rounds.Add(
                    new Round(
                        game.ActiveAgents.Select(agent => {
                            var action = agent.PickAction(state);
                            if (action == null)
                            {
                                game.Disable(agent);
                                return null;
                            }
                            return new Event(agent, action);
                            }).Where(e => e != null).ToList(),
                        roundIndex
                        )
                    );
            }

            rounds[roundIndex].ApplyAll(game);

            if (newRound)
            {
                rounds[roundIndex].CalculateScores(game);

                foreach (var score in rounds[roundIndex].GetScores())
                {
                    if (scoreboard.ContainsKey(score.agent))
                        scoreboard[score.agent] += score.score;
                    else
                        scoreboard[score.agent] = score.score;
                }
            }
                
        }

        public void StepBackward()
        {
            if (roundIndex < 0)
                return;

            game.DespawnAgents(roundIndex);
            rounds[roundIndex].UndoAll(game);
            roundIndex--;
        }

        public IState GetCurrentStep()
        {
            var state = game.GenerateState();
            state.Winner = winCondition.GetWinner(roundIndex);
            return state;
        }

        public int CurrentStep()
        {
            return roundIndex;
        }

        public int CountSteps()
        {
            return rounds.Count;
        }

        public IEnumerable<IAgent> GetAgents()
        {
            return game.ActiveAgents;
        }

        public void ReWind()
        {
            while(roundIndex >= 0)
            {
                StepBackward();
            }
        }

        public IReadOnlyDictionary<IAgent, float> GetScores()
        {
            return (IReadOnlyDictionary<IAgent, float>)scoreboard;
        }
    }
}
