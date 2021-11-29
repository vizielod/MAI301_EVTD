using BehaviorTree.NodeBase;
using Simulator.actioncommands;
using System.Linq;

namespace BehaviorTree.ConditionalNodes
{
    class CanMoveNorth:LeafNode
    {
        private readonly EnemyBlackboard blackboard;

        public CanMoveNorth(EnemyBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        public override bool CheckConditions()
        {
            return blackboard.LegalActions != null && blackboard.LegalActions.Any(a => a is GoNorth);
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
