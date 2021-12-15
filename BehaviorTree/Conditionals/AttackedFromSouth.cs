using BehaviorTree.NodeBase;

namespace BehaviorTree.Conditionals
{
    class AttackedFromSouth : IConditionStrategy
    {
        public bool HandleEnemy(EnemyBlackboard blackboard)
        {
            foreach (var turret in blackboard.AttackingTurrets)
            {
                if (turret.Value.HasFlag(Simulator.Direction.South))
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
