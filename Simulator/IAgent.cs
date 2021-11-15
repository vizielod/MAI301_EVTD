namespace Simulator
{
    public interface IAgent
    {
        IAction PickAction(IState state, IAction previousAction);
        (int x, int y) InitialPosition { get; }

    }
}