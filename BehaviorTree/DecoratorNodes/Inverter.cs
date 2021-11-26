using BehaviorTree.NodeBase;

namespace BehaviorTree.DecoratorNodes
{
    public class Inverter : DecoratorNode
    {
        public Inverter( Blackboard blackboard, Node node) : base( blackboard, node)
        {
        }

        public override void DoAction()
        {
            node.DoAction();

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
