using System.Collections.Generic;
using System.Linq;
using Simulator.state;

namespace Simulator
{
    public class Simulator : IStateSequence
    {
        private List<State> states;
        int iterator;
        private readonly State initialState;

        internal Simulator(State initialState)
        {
            this.initialState = initialState;
        }

        public void StepForward()
        {
            iterator++;
            if (iterator > states.Count)
            {
                var state = states.Last();
                
            }
        }

        public void StepBackward()
        {

        }

        public IState GetCurrentStep()
        {
            throw new System.NotImplementedException();
        }

        public void AddAgent(IAgent agent)
        {
            throw new System.NotImplementedException();
            //states[iterator].AddAgent(agent);
        }

        public int CurrentStep()
        {
            return iterator;
        }

        public int CountSteps()
        {
            return states.Count;
        }

        public IEnumerable<IAgent> GetAgents()
        {
            return states[iterator].Agents;
        }
    }
}
