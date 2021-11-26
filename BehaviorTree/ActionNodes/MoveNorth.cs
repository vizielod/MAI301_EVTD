using BehaviorTree.NodeBase;
using Simulator.actioncommands;

namespace BehaviorTree.ActionNodes
{
    class MoveNorth : LeafNode
    {
        public MoveNorth( Blackboard blackboard) : base( blackboard)
        { }

        public override bool CheckConditions()
        {
            return true;
        }

        public override void DoAction()
        {
            blackboard.ChoosenAction = new GoNorth();
            controller.FinishWithSuccess();
        }
    }
}
