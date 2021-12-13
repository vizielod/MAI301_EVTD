using BehaviorTree.NodeBase;
using Simulator.actioncommands;

namespace BehaviorTree.Actions
{
    class MoveNorth : IActionStrategy
    {
        public bool HandleEnemy(EnemyBlackboard blackboard)
        {
            blackboard.ChoosenAction = new GoNorth();
            return true;
        }

        public bool HandleTurret(TurretBlackboard blackboard)
        {
            return false;
        }
    }
}
