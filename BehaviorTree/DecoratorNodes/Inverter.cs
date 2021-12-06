using BehaviorTree.NodeBase;

namespace BehaviorTree.DecoratorNodes
{
    class Inverter : DecoratorNode
    {
        public Inverter( Node node) : base( node)
        {
        }

        public override void DoAction(Blackboard blackboard)
        {
            node.DoAction(blackboard);

            if (node.GetControl().Succeeded())
            {
                GetControl().FinishWithFailure();
            }
            else 
            {
                GetControl().FinishWithSuccess();
            }
        }
    }
}
