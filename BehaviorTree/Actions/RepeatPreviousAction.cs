using BehaviorTree.NodeBase;

namespace BehaviorTree.Actions
{
    class RepeatPreviousAction : IActionStrategy
    {
        public bool HandleEnemy(EnemyBlackboard blackboard)
        {
            blackboard.ChoosenAction = blackboard.PreviousAction;
            return true;
        }

        public bool HandleTurret(TurretBlackboard blackboard)
        {
            return false;
        }
    }
}
