using BehaviorTree.NodeBase;
using Simulator.actioncommands;

namespace BehaviorTree.Actions
{
    class MoveSouth : IActionStrategy
    {
        public bool HandleEnemy(EnemyBlackboard blackboard)
        {
            blackboard.ChoosenAction = new GoSouth();
            return true;
        }

        public bool HandleTurret(TurretBlackboard blackboard)
        {
            return false;
        }
    }
}
