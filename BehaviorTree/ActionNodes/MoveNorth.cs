using BehaviorTree.NodeBase;
using Simulator.actioncommands;

namespace BehaviorTree.ActionNodes
{
    class MoveNorth : LeafNode
    {
        public MoveNorth()
        {
        }

        public override bool CheckConditions()
        {
            return true;
        }

        public override void HandleEnemy(EnemyBlackboard blackboard)
        {
            blackboard.ChoosenAction = new GoNorth();
            controller.FinishWithSuccess();
        }

        public override void HandleTurret(TurretBlackboard blackboard)
        {
            controller.FinishWithFailure();
        }
    }
}
