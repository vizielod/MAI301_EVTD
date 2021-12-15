using BehaviorTree.NodeBase;
using Simulator.actioncommands;

namespace BehaviorTree.Actions
{
    class Fire : IActionStrategy
    {
        public bool HandleEnemy(EnemyBlackboard blackboard)
        {
            return false;
        }

        public bool HandleTurret(TurretBlackboard blackboard)
        {
            if (blackboard.ClosestEnemy != null && blackboard.IsEnemyInRange)
            {
                blackboard.ChoosenAction = new QuickAttack(blackboard.Damage, blackboard.ClosestEnemy, blackboard.PreviousTargetEnemy);
                blackboard.PreviousTargetEnemy = blackboard.ClosestEnemy;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
