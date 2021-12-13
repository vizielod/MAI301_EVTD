using BehaviorTree.NodeBase;
using Simulator.actioncommands;

namespace BehaviorTree.Actions
{
    class MoveWest : IActionStrategy
    {
        public bool HandleEnemy(EnemyBlackboard blackboard)
        {
            blackboard.ChoosenAction = new GoWest();
            return true;
        }

        public bool HandleTurret(TurretBlackboard blackboard)
        {
            return false;
        }
    }
}
