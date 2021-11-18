namespace Simulator
{
    public interface IAction
    {
        void Apply(IStateObject stateObject);
        void Undo(IStateObject stateObject);
    }
}