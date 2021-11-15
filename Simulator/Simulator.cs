using System.Collections.Generic;
using System.Linq;
using Simulator.state;

namespace Simulator
{
    public class Simulator : IStateSequence
    {
        private readonly List<Round> rounds;
        private readonly IGame game;
        int round;

        internal Simulator(IGame game)
        {
            this.game = game;
            rounds = new List<Round>();
            round = -1;
        }

        public void StepForward()
        {
            round++;
            if (round >= rounds.Count)
            {
                rounds.Add(
                    new Round(
                        game.Agents.Select(a => new Event(a, a.PickAction(game.GenerateState())))
                        )
                    );
            }
            rounds[round].ApplyAll(game);
        }

        public void StepBackward()
        {
            if (round < 0)
                return;

            rounds[round].UndoAll(game);
            round--;
        }

        public IState GetCurrentStep()
        {
            return game.GenerateState();
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
            return game.Agents;
        }
    }
}
