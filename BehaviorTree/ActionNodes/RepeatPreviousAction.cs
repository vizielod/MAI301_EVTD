using BehaviorTree.NodeBase;

namespace BehaviorTree.ActionNodes
{
    public class RepeatPreviousAction:LeafNode
    {
        public RepeatPreviousAction( Blackboard bb):base(bb)
        {
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
