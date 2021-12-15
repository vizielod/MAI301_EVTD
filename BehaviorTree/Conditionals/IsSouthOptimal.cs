using System;
using BehaviorTree.NodeBase;
using Simulator.actioncommands;

namespace BehaviorTree.Conditionals
{
    class IsSouthOptimal : IConditionStrategy
    {
        public bool HandleEnemy(EnemyBlackboard blackboard)
        {
            return (blackboard.ProgressiveAction is GoSouth);
        }

        public bool HandleTurret(TurretBlackboard blackboard)
        {
            return false;
        }
    }
}
