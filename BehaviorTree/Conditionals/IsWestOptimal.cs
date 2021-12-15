using System;
using BehaviorTree.NodeBase;
using Simulator.actioncommands;

namespace BehaviorTree.Conditionals
{
    class IsWestOptimal: IConditionStrategy
    {

        public bool HandleEnemy(EnemyBlackboard blackboard)
        {
            return (blackboard.ProgressiveAction is GoWest);
        }

        public bool HandleTurret(TurretBlackboard blackboard)
        {
            return false;
        }
    }
}
