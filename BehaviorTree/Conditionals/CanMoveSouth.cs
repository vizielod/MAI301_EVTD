using BehaviorTree.NodeBase;
using Simulator.actioncommands;
using System.Linq;

namespace BehaviorTree.Conditionals
{
    class CanMoveSouth : IConditionStrategy
    {
        public bool HandleEnemy(EnemyBlackboard blackboard)
        {
            return blackboard.LegalActions != null && blackboard.LegalActions.Any(a => a is GoSouth);
        }

        public bool HandleTurret(TurretBlackboard blackboard)
        {
            return false;
        }
    }
}
