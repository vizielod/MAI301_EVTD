using BehaviorTree.NodeBase;
using Simulator.actioncommands;

namespace BehaviorTree.ActionNodes
{
    class Fire : LeafNode
    {
        private readonly TurretBlackboard blackboard;

        public Fire(TurretBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        public override bool CheckConditions()
        {
            return blackboard.ClosestEnemy != null && blackboard.IsEnemyInRange; 
        }

        public override void DoAction()
        {
            if (CheckConditions())
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
