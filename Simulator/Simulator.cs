using System.Collections.Generic;
using System.Linq;
using Simulator.state;

namespace Simulator
{
    public class Simulator : IStateSequence
    {
        private State _currentState;
        private readonly List<Round> _rounds;
        int _round;
        private readonly State _initialState;

        internal Simulator(State initialState)
        {
            _initialState = initialState;
            _currentState = initialState;
            _rounds = new List<Round>();
            _round = 0;
        }

        public void StepForward()
        {
            _round++;
            if (_round >= _rounds.Count)
            {
                _rounds.Add(
                    new Round(
                        _currentState.Agents.Select(a => new Event(a, a.PickAction(_currentState)))
                        )
                    );
            }
            _rounds[_round].ApplyAll();
        }

        public void StepBackward()
        {
            if (_round < 0)
                return;

            _rounds[_round].UndoAll();
            _round--;
        }

        public IState GetCurrentStep()
        {
            return _currentState;
        }

        public int CurrentStep()
        {
            return _round;
        }

        public int CountSteps()
        {
            return _rounds.Count;
        }

        public IEnumerable<IAgent> GetAgents()
        {
            return _currentState.Agents;
        }
    }
}
