using BehaviorTree.NodeBase;

namespace BehaviorTree.ConditionalNodes
{
    class IsAttackingTurretEast:LeafNode
    {
        public IsAttackingTurretEast()
        {
        }

        public override bool CheckConditions()
        {
            return true;
        }

        public override void HandleEnemy(EnemyBlackboard blackboard)
        {
            if (blackboard.ClosestTurretPosition != null && blackboard.CurrentPosition != null)
            {
                int y = blackboard.ClosestTurretPosition.Value.y - blackboard.CurrentPosition.Value.y;

                if (y == 1)
                {
                    controller.FinishWithSuccess();
                }
            }
            else
            {
                controller.FinishWithFailure();
            }
        }

        public override void HandleTurret(TurretBlackboard blackboard)
        {
            controller.FinishWithFailure();
        }
    }
}
