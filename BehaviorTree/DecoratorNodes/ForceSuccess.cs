using BehaviorTree.NodeBase;

namespace BehaviorTree.DecoratorNodes
{
    class ForceSuccess : DecoratorNode
    {
        public ForceSuccess(Node node) : base(node)
        {
        }

        public override void DoAction()
        {
            node.DoAction();

            GetControl().FinishWithSuccess();
        }
    }
}
