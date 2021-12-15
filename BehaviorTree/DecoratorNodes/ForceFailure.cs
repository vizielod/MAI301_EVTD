using BehaviorTree.NodeBase;

namespace BehaviorTree.DecoratorNodes
{
    class ForceFailure : DecoratorNode
    {
        public ForceFailure( Node node):base(node)
        {
        }

        public override Node DeepCopy()
        {
            return new ForceFailure(node.DeepCopy());
        }

        public override void DoAction(Blackboard blackboard)
        {
            node.DoAction(blackboard);

            GetControl().FinishWithFailure();
        }
    }
}
