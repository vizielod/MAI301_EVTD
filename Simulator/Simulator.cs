﻿using System.Collections.Generic;
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
            game.SpawnAgents(round);
            var newRound = round >= rounds.Count;
            
            if (newRound)
            {
                IState state = game.GenerateState();
                rounds.Add(
                    new Round(
                        game.Agents.Select(a => new Event(a, a.PickAction(state))).ToList()
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
