using BehaviorTree.NodeBase;

namespace BehaviorTree.ConditionalNodes
{
    class IsAttackingTurretNorth:LeafNode
    {

        public IsAttackingTurretNorth()
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
                int x = blackboard.ClosestTurretPosition.Value.x - blackboard.CurrentPosition.Value.x;

                if (x == -1)
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
