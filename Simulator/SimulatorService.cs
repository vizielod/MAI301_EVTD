using System.Collections.Generic;

namespace Simulator
{
    class SimulatorService
    {
        List<State> states;
        int iterator;

        public void StepForward()
        {

        }

        public void StepBackward()
        {

        }

        public void AddAgent(IAgent agent)
        {
            states[iterator].addAgent(agent);
        } 

        public int CurrentStep()
        {
            return 0;
        }

        public IEnumerable<IAgent> GetAgents()
        {
            return null;
        }

    }
}
