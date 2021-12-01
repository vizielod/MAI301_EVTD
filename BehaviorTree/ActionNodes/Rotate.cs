using BehaviorTree.NodeBase;
using Simulator.actioncommands;

namespace BehaviorTree.ActionNodes
{
    class Rotate: LeafNode
    {
        private readonly EnemyBlackboard blackboard;

        public Rotate( EnemyBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        public override bool CheckConditions()
        {
            return true;
        }

        public override void DoAction()
        {
            blackboard.ChoosenAction = new Turn();
            controller.FinishWithSuccess();
        }
    }
}

