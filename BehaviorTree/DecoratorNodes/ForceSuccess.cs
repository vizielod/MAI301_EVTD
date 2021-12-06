using BehaviorTree.NodeBase;

namespace BehaviorTree.DecoratorNodes
{
    class ForceSuccess : DecoratorNode
    {
        public ForceSuccess(Node node) : base(node)
        {
        }

        public override void DoAction(Blackboard blackboard)
        {
            node.DoAction(blackboard);

            GetControl().FinishWithSuccess();
        }
    }
}
