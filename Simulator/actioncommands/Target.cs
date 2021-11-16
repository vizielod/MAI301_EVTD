namespace Simulator.actioncommands
{
    public sealed class Target : IAction
    {
        private readonly IAgent target;
        private readonly IAgent previousTarget;

        public Target(IAgent target, IAgent previousTarget)
        {
            this.target = target;
            this.previousTarget = previousTarget;
        }

        public void Apply(IStateObject stateObject)
        {
            stateObject.Target = target;
        }

        public void Undo(IStateObject stateObject)
        {
            stateObject.Target = previousTarget;
        }
    }
}
