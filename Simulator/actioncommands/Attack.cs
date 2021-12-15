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
            stateObject.EngagedTarget = true;
        }

        void IAction.Undo(IStateObject stateObject)
        {
            stateObject.Target.Heal(damage);
            stateObject.EngagedTarget = true;
        }
    }
}
