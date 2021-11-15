namespace Simulator
{
    public interface IAgent
    {
        IAction PickAction(IState state);
        (int x, int y) InitialPosition { get; }

    }
}