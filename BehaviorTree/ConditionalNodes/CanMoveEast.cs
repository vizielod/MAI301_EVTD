using BehaviorTree.NodeBase;
using Simulator.actioncommands;
using System.Linq;

namespace BehaviorTree.ConditionalNodes
{
    class CanMoveEast : LeafNode
    {
        private readonly EnemyBlackboard blackboard;

        public CanMoveEast(EnemyBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        public override bool CheckConditions()
        {
            return blackboard.LegalActions != null && blackboard.LegalActions.Any(a => a is GoEast);
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
