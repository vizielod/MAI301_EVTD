namespace Simulator.actioncommands
{
    public sealed class QuickAttack : IAction
    {
        private readonly int damage;
        private readonly IAgent target;

        public QuickAttack(int damage, IAgent target)
        {
            this.damage = damage;
            this.target = target;
        }

        void IAction.Apply(IStateObject stateObject)
        {
            target.Damage(damage);
        }

        void IAction.Undo(IStateObject stateObject)
        {
            target.Heal(damage);
        }
    }
}
