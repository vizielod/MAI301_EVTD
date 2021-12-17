using BehaviorTree.NodeBase;
using Simulator.actioncommands;

namespace BehaviorTree.Actions
{
    class Fire : IActionStrategy
    {
        public ResultEnum Result { get; set; }

        public void HandleEnemy(EnemyBlackboard blackboard)
        {
            Result = ResultEnum.Failed;
        }

        public void HandleTurret(TurretBlackboard blackboard)
        {
            if (blackboard.ClosestEnemy != null && blackboard.IsEnemyInRange)
            {
                blackboard.ChoosenAction = new QuickAttack(blackboard.Damage, blackboard.ClosestEnemy, blackboard.PreviousTargetEnemy);
                blackboard.PreviousTargetEnemy = blackboard.ClosestEnemy;
                Result = ResultEnum.Succeeded;
            }
            else
            {
                Result = ResultEnum.Failed;
            }
        }
    }
}
