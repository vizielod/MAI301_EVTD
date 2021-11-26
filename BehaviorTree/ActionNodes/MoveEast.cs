using BehaviorTree.NodeBase;
using Simulator.actioncommands;

namespace BehaviorTree.ActionNodes
{
    class MoveEast : LeafNode
    {
        public MoveEast(Blackboard blackboard):base( blackboard) 
        { }

        public override bool CheckConditions()
        {
            return true;
        }

        public override void DoAction()
        {
            blackboard.ChoosenAction = new GoEast();
            controller.FinishWithSuccess(); 
        }
    }
}
