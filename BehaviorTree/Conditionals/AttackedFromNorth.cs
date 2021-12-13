using BehaviorTree.NodeBase;

namespace BehaviorTree.Conditionals
{
    class AttackedFromNorth : IConditionStrategy
    {

        public bool HandleEnemy(EnemyBlackboard blackboard)
        {
            if (blackboard.ClosestTurretPosition != null && blackboard.CurrentPosition != null)
            {
                int x = blackboard.ClosestTurretPosition.Value.x - blackboard.CurrentPosition.Value.x;

                if (x == -1)
                {
                    return true;
                }
            }
            return false;
        }

        public bool HandleTurret(TurretBlackboard blackboard)
        {
            return false;
        }
    }
}
