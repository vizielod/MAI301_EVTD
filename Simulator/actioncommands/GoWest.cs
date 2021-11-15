using Simulator.state;

namespace Simulator.actioncommands
{
    public sealed class GoWest : IAction
    {
        void IAction.Apply(IStateObject stateObject)
        {
            stateObject.Move(-1, 0);
        }

        void IAction.Undo(IStateObject stateObject)
        {
            stateObject.Move(1, 0);
        }

        public override bool Equals(object obj)
        {
            return obj.GetType() == typeof(GoEast);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
