using BehaviorTree.NodeBase;
using Simulator.actioncommands;

namespace BehaviorTree.ActionNodes
{
    class MoveWest : LeafNode
    {
        public MoveWest( Blackboard blackboard) : base( blackboard)
        { }

        public override bool CheckConditions()
        {
            return true;
        }

        public override void DoAction()
        {
            blackboard.ChoosenAction = new GoWest();
            controller.FinishWithSuccess();
        }
    }
}
