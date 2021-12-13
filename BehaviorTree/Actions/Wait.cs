using System;
using BehaviorTree.NodeBase;
using Simulator.actioncommands;

namespace BehaviorTree.Actions
{
    class Wait : IActionStrategy
    {
        public bool HandleEnemy(EnemyBlackboard blackboard)
        {

            blackboard.ChoosenAction = new Idle();
            return true;
            
        }

        public bool HandleTurret(TurretBlackboard blackboard)
        {
            return false;
        }
    }
}
