using BehaviorTree.NodeBase;

namespace BehaviorTree.ActionNodes
{
    class RepeatPreviousAction:LeafNode
    {
        public RepeatPreviousAction()
        {
        }

        public override bool CheckConditions()
        {
            return true;
        }

        public override void HandleEnemy(EnemyBlackboard blackboard)
        {
            blackboard.ChoosenAction = blackboard.PreviousAction;
            controller.FinishWithSuccess();
        }

        public override void HandleTurret(TurretBlackboard blackboard)
        {
            controller.FinishWithFailure();
        }
    }
}
