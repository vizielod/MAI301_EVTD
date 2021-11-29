using BehaviorTree.NodeBase;
using Simulator.actioncommands;

namespace BehaviorTree.ActionNodes
{
    class MoveSouth : LeafNode
    {
        private readonly EnemyBlackboard blackboard;

        public MoveSouth( EnemyBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

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
