namespace Simulator.actioncommands
{
    public sealed class GoNorth : IAction
    {
        void IAction.Apply(IStateObject stateObject)
        {
            stateObject.Move(0, 1);
        }

        void IAction.Undo(IStateObject stateObject)
        {
            stateObject.Move(0, -1);
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
