using Simulator.state;

namespace Simulator.actioncommands
{
    public sealed class GoEast : IAction
    {
        void IAction.Apply(IStateObject stateObject)
        {
            stateObject.Move(1, 0);
        }

        void IAction.Undo(IStateObject stateObject)
        {
            stateObject.Move(-1, 0);
        }
    }
}
