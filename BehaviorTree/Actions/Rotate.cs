using BehaviorTree.NodeBase;
using Simulator.actioncommands;

namespace BehaviorTree.Actions
{
    class Rotate : IActionStrategy
    {
        public bool HandleEnemy(EnemyBlackboard blackboard)
        {
            return false;
        }

        public bool HandleTurret(TurretBlackboard blackboard)
        {
            blackboard.ChoosenAction = new Turn();
            return true;
        }
    }
}

