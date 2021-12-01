using BehaviorTree.NodeBase;

namespace BehaviorTree.ConditionalNodes
{
    class IsAttackingTurretEast:LeafNode
    {
        private readonly EnemyBlackboard blackboard;

        public IsAttackingTurretEast(EnemyBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        public override bool CheckConditions()
        {
            return blackboard.ClosestTurretPosition != null && blackboard.CurrentPosition != null;
        }

        public override void DoAction()
        {
            if (CheckConditions())
            {
                int y = blackboard.ClosestTurretPosition.Value.y - blackboard.CurrentPosition.Value.y;

                if (y==1)
                {
                    controller.FinishWithSuccess();
                }
            }
            else
            {
                controller.FinishWithFailure();
            }
        }
    }
}
