using BehaviorTree.NodeBase;
using System.Linq;

namespace BehaviorTree.ConditionalNodes
{
    class CanRepeatLastMove:LeafNode
    {
        private readonly EnemyBlackboard blackboard;

        public CanRepeatLastMove(EnemyBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        public override bool CheckConditions()
        {
            return blackboard.LegalActions != null && blackboard.LegalActions.Contains(blackboard.PreviousAction);
        }

        public override void DoAction()
        {
            if (CheckConditions())
            {
                controller.FinishWithSuccess();
            }
            else
            {
                controller.FinishWithFailure();
            }
        }
    }
}
