using BehaviorTree.NodeBase;

namespace BehaviorTree.Conditionals
{
    class AttackedFromWest : IConditionStrategy
    {
        public bool HandleEnemy(EnemyBlackboard blackboard)
        {
            foreach (var turret in blackboard.AttackingTurrets)
            {
                if (turret.Value.HasFlag(Simulator.Direction.West))
                    return true;
            }

            return false;
        }

        public bool HandleTurret(TurretBlackboard blackboard)
        {
            return false;
        }
    }
}
