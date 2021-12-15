using BehaviorTree.NodeBase;
using Simulator.actioncommands;

namespace BehaviorTree.Actions
{
    class MoveForward : IActionStrategy
    {
        public bool HandleEnemy(EnemyBlackboard blackboard)
        {
            if (blackboard.ProgressiveAction is Idle)
                return false;

            blackboard.ChoosenAction = blackboard.ProgressiveAction;
            return true;
        }

        public bool HandleTurret(TurretBlackboard blackboard)
        {
            return false;
        }
    }
}
