using BehaviorTree.NodeBase;

namespace BehaviorTree.DecoratorNodes
{
    class ForceFailure : DecoratorNode
    {
        public ForceFailure( Node node):base(node)
        {
        }

        public override void DoAction(Blackboard blackboard)
        {
            node.DoAction(blackboard);

            GetControl().FinishWithFailure();
        }
    }
}
