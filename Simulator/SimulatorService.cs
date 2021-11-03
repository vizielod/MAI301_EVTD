using System.Collections.Generic;
using Simulator.state;

namespace Simulator
{
    public class SimulatorService : IStateSequence
    {
        private List<State> states;
        int iterator;

        public void StepForward()
        {
            throw new System.NotImplementedException();
        }

        public void StepBackward()
        {
            throw new System.NotImplementedException();
        }

        public IState GetCurrentStep()
        {
            throw new System.NotImplementedException();
        }

        public void AddAgent(IAgent agent)
        {
            states[iterator].AddAgent(agent);
        }

        public int CurrentStep()
        {
            return 0;
        }

        public int CountSteps()
        {
            return 0;
        }

        public IEnumerable<IAgent> GetAgents()
        {
            return null;
        }
    }
}
