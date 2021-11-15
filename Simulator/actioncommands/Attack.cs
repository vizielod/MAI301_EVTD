namespace Simulator.actioncommands
{
    public sealed class Attack : IAction
    {
        private readonly int damage;

        public Attack(int damage)
        {
            this.damage = damage;
        }

        void IAction.Apply(IStateObject stateObject)
        {
            stateObject.Target.Damage(damage);
        }

        void IAction.Undo(IStateObject stateObject)
        {
            stateObject.Target.Heal(damage);
        }
    }
}
