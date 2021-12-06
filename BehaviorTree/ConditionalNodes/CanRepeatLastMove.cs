using BehaviorTree.NodeBase;
using System.Linq;

namespace BehaviorTree.ConditionalNodes
{
    class CanRepeatLastMove:LeafNode
    {
        public CanRepeatLastMove()
        {
        }

        public override bool CheckConditions()
        {
            return true;
        }

        public override void HandleEnemy(EnemyBlackboard blackboard)
        {
            if (blackboard.LegalActions != null && blackboard.LegalActions.Contains(blackboard.PreviousAction))
            {
                controller.FinishWithSuccess();
            }
            else
            {
                controller.FinishWithFailure();
            }
        }

        public override void HandleTurret(TurretBlackboard blackboard)
        {
            controller.FinishWithFailure();
        }
    }
}
