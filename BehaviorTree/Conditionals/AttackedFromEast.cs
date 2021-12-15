using BehaviorTree.NodeBase;

namespace BehaviorTree.Conditionals
{
    class AttackedFromEast : IConditionStrategy
    {
        public bool HandleEnemy(EnemyBlackboard blackboard)
        {
            foreach (var turret in blackboard.AttackingTurrets)
            {
                if (turret.Value.HasFlag(Simulator.Direction.East))
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
