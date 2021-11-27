using BehaviorTree.NodeBase;
using Simulator.actioncommands;

namespace BehaviorTree.ActionNodes
{
    class MoveEast : LeafNode
    {
        private readonly EnemyBlackboard blackboard;

        public MoveEast(EnemyBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

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
