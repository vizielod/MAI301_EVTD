using BehaviorTree.NodeBase;
using System.Linq;

namespace BehaviorTree.Conditionals
{
    class CanRepeatLastMove : IConditionStrategy
    {
        public bool HandleEnemy(EnemyBlackboard blackboard)
        {
            return blackboard.LegalActions != null && blackboard.LegalActions.Contains(blackboard.PreviousAction);
        }

        public bool HandleTurret(TurretBlackboard blackboard)
        {
            return false;
        }
    }
}
