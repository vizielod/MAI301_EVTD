using BehaviorTree.NodeBase;

namespace BehaviorTree.Conditionals
{
    class AttackedFromNorth : IConditionStrategy
    {

        public bool HandleEnemy(EnemyBlackboard blackboard)
        {
            foreach (var turret in blackboard.AttackingTurrets)
            {
                if (turret.Value.HasFlag(Simulator.Direction.North))
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
