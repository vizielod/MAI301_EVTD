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
        IDictionary<IAgent, float> totalScoreboard;
        int round;

        public IEnumerable<IAgent> AllAgents => game.AllAgents;

        public IEnumerable<IAgent> AllEnemyAgents => game.AllEnemyAgents;

        public bool IsGameOver => winCondition.GetWinner(round).HasValue;

        internal Simulator(IGame game)
        {
            this.game = game;
            rounds = new List<Round>();
            round = -1;
            totalScoreboard = new Dictionary<IAgent, float>();
            winCondition = new WinConditionChain(new EnemiesDefeatedWinCondition(game), new WinConditionChain(new EnemiesGoalReachedWinCondition(game), new TimeoutWinCondition(200)));

        }

        public void StepForward()
        {
            if (IsGameOver)
            {
                return;
            }

            round++;
            game.SpawnAgents(round);

            

            var newRound = round >= rounds.Count;
            
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
                        round
                        )
                    );
            }

            rounds[round].ApplyAll(game);

            if (newRound)
                rounds[round].ScoreAll(game);
        }

        public void StepBackward()
        {
            if (round < 0)
                return;

            game.DespawnAgents(round);
            rounds[round].UndoAll(game);
            round--;
        }

        public IState GetCurrentStep()
        {
            var state = game.GenerateState();
            state.Winner = winCondition.GetWinner(round);
            return state;
        }

        public int CurrentStep()
        {
            return round;
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
            while(round >= 0)
            {
                StepBackward();
            }
        }

        public IDictionary<IAgent, float> GetScores()
        {
            return new Dictionary<IAgent, float>();
        }
    }
}
