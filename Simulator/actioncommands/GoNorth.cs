namespace Simulator.actioncommands
{
    public sealed class GoNorth : IAction
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
            return obj != null && obj.GetType() == this.GetType();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
