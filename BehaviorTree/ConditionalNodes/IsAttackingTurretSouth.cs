using BehaviorTree.NodeBase;

namespace BehaviorTree.ConditionalNodes
{
    class IsAttackingTurretSouth:LeafNode
    {
        private readonly EnemyBlackboard blackboard;

        public IsAttackingTurretSouth(EnemyBlackboard blackboard)
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
                int x = blackboard.ClosestTurretPosition.Value.x - blackboard.CurrentPosition.Value.x;

                if (x == 1)
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
