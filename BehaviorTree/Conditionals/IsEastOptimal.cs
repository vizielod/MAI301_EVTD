using System;
using BehaviorTree.NodeBase;
using Simulator.actioncommands;

namespace BehaviorTree.Conditionals
{
    class IsEastOptimal: IConditionStrategy
    {

        public bool HandleEnemy(EnemyBlackboard blackboard)
        {
            return (blackboard.ProgressiveAction is GoEast);
        }

        public bool HandleTurret(TurretBlackboard blackboard)
        {
            return false;
        }
    }
}
