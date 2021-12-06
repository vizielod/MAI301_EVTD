using BehaviorTree.NodeBase;
using Simulator.actioncommands;

namespace BehaviorTree.ActionNodes
{
    class Fire : LeafNode
    {
        public Fire()
        {
        }

        public override bool CheckConditions()
        {
            return true; 
        }

        public override void HandleEnemy(EnemyBlackboard blackboard)
        {
            controller.FinishWithFailure();
        }

        public override void HandleTurret(TurretBlackboard blackboard)
        {
            if (blackboard.ClosestEnemy != null && blackboard.IsEnemyInRange)
            {
                blackboard.ChoosenAction = new QuickAttack(blackboard.Damage, blackboard.ClosestEnemy, blackboard.PreviousTargetEnemy);
                blackboard.PreviousTargetEnemy = blackboard.ClosestEnemy;
                controller.FinishWithSuccess();
            }
            else
            {
                controller.FinishWithFailure();
            }
        }
    }
}
