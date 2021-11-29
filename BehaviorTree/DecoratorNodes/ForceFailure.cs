using BehaviorTree.NodeBase;

namespace BehaviorTree.DecoratorNodes
{
    class ForceFailure : DecoratorNode
    {
        public ForceFailure( Node node):base(node)
        {
        }

        public override void DoAction()
        {
            node.DoAction();

            GetControl().FinishWithFailure();
        }
    }
}
