namespace Simulator.actioncommands
{
    public sealed class QuickAttack : IAction
    {
        private readonly int damage;
        private readonly IAgent target;
        private readonly IAgent previousTarget;

        public QuickAttack(int damage, IAgent target, IAgent previousTarget)
        {
            this.damage = damage;
            this.target = target;
            this.previousTarget = previousTarget;
        }

        void IAction.Apply(IStateObject stateObject)
        {
            stateObject.Target = target;
            stateObject.Target.Damage(damage);
            stateObject.EngagedTarget = true;
        }

        void IAction.Undo(IStateObject stateObject)
        {
            stateObject.Target.Heal(damage);
            stateObject.Target = previousTarget;
            if (previousTarget != null)
                stateObject.EngagedTarget = true;
        }
    }
}
