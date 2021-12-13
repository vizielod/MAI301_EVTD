using BehaviorTree.NodeBase;

namespace BehaviorTree.Conditionals
{
    class AttackedFromWest : IConditionStrategy
    {
        public bool HandleEnemy(EnemyBlackboard blackboard)
        {
            if (blackboard.ClosestTurretPosition != null && blackboard.CurrentPosition != null)
            {
                int y = blackboard.ClosestTurretPosition.Value.y - blackboard.CurrentPosition.Value.y;

                if (y == -1)
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
