namespace Simulator
{
    interface IStateSequence
    {
        void StepForward();
        void StepBackward();
        IState GetCurrentStep();
    }
}
