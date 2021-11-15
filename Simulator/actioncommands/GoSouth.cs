using Simulator.state;

namespace Simulator.actioncommands
{
    public sealed class GoSouth : IAction
    {
        void IAction.Apply(IStateObject stateObject)
        {
            stateObject.Move(0, -1);
        }

        void IAction.Undo(IStateObject stateObject)
        {
            stateObject.Move(0, 1);
        }
    }
}
