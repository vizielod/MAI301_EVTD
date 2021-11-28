using System.Collections.Generic;

namespace Simulator
{
    public interface IStateSequence
    {
        void StepForward();
        void StepBackward();
        IState GetCurrentStep();
        IEnumerable<IAgent> AllAgents { get; }
        bool IsGameOver { get; }
    }
}
