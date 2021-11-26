using BehaviorTree.NodeBase;
using Simulator.actioncommands;
using System.Linq;

namespace BehaviorTree.ConditionalNodes
{
    class CanGoSouth:LeafNode
    {
        public CanGoSouth(Blackboard blackboard) : base(blackboard)
        {
        }

        public override bool CheckConditions()
        {
            return blackboard.LegalActions != null && blackboard.LegalActions.Any(a => a is GoSouth);
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
