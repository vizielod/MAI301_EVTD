using BehaviorTree.NodeBase;

namespace BehaviorTree.ActionNodes
{
    class RepeatPreviousAction:LeafNode
    {
        private readonly EnemyBlackboard blackboard;

        public RepeatPreviousAction( EnemyBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        public override bool CheckConditions()
        {
            return true;
        }

        public override void DoAction()
        {
            blackboard.ChoosenAction = blackboard.PreviousAction;
            controller.FinishWithSuccess();
        }
    }
}
