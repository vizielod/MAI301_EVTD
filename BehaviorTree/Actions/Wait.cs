using System;
using BehaviorTree.NodeBase;
using Simulator.actioncommands;

namespace BehaviorTree.Actions
{
    class Wait : IActionStrategy
    {
        public ResultEnum Result { get; set; }

        public void HandleEnemy(EnemyBlackboard blackboard)
        {

            blackboard.ChoosenAction = new Idle();
            Result = ResultEnum.Succeeded;
            
        }

        public void HandleTurret(TurretBlackboard blackboard)
        {
            Result = ResultEnum.Failed;
        }

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}
