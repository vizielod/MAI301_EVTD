using BehaviorTree.NodeBase;
using Simulator.actioncommands;

namespace BehaviorTree.ActionNodes
{
    class MoveWest : LeafNode
    {
        private readonly EnemyBlackboard blackboard;

        public MoveWest( EnemyBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

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
