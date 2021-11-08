namespace Simulator
{
    public interface IStateSequence
    {
        void StepForward();
        void StepBackward();
        IState GetCurrentStep();
    }
}
