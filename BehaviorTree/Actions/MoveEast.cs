using BehaviorTree.NodeBase;
using Simulator.actioncommands;

namespace BehaviorTree.Actions
{
    class MoveEast : IActionStrategy
    {
        public bool HandleEnemy(EnemyBlackboard blackboard)
        {
            blackboard.ChoosenAction = new GoEast();
            return true;
        }

        public bool HandleTurret(TurretBlackboard blackboard)
        {
            return false;
        }
    }
}
