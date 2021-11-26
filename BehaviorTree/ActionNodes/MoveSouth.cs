using BehaviorTree.NodeBase;
using Simulator.actioncommands;

namespace BehaviorTree.ActionNodes
{
    class MoveSouth : LeafNode
    {
        public MoveSouth( Blackboard blackboard) : base( blackboard)
        { }

        public override bool CheckConditions()
        {
            return true;
        }
        public override void DoAction()
        {
            blackboard.ChoosenAction = new GoSouth();
            controller.FinishWithSuccess();
        }
    }
}
