using BehaviorTree.NodeBase;
using Simulator.actioncommands;

namespace BehaviorTree.ActionNodes
{
    class MoveNorth : LeafNode
    {
        private readonly EnemyBlackboard blackboard;

        public MoveNorth( EnemyBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

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
