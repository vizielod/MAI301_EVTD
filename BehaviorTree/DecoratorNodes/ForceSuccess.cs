using BehaviorTree.NodeBase;

namespace BehaviorTree.DecoratorNodes
{
    class ForceSuccess : DecoratorNode
    {
        public ForceSuccess(Node node) : base(node)
        {
        }

        public override Node DeepCopy()
        {
            return new ForceSuccess(node.DeepCopy());
        }

        public override void DoAction(Blackboard blackboard)
        {
            node.DoAction(blackboard);

            GetControl().FinishWithSuccess();
        }
    }
}
