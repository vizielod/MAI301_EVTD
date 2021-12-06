using BehaviorTree.NodeBase;
using Simulator.actioncommands;

namespace BehaviorTree.ActionNodes
{
    class MoveWest : LeafNode
    {
        public MoveWest()
        {
        }

        public override bool CheckConditions()
        {
            return true;
        }

        public override void HandleEnemy(EnemyBlackboard blackboard)
        {
            blackboard.ChoosenAction = new GoWest();
            controller.FinishWithSuccess();
        }

        public override void HandleTurret(TurretBlackboard blackboard)
        {
            controller.FinishWithFailure();
        }
    }
}
