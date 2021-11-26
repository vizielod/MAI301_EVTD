using BehaviorTree.NodeBase;

namespace BehaviorTree.DecoratorNodes
{
    class ForceSuccess : DecoratorNode
    {
        public ForceSuccess(Blackboard blackboard, Node node) : base( blackboard, node)
        {
        }

        public override void DoAction()
        {
            node.DoAction();

            GetControl().FinishWithSuccess();
        }
    }
}
