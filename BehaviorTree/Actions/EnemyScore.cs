using BehaviorTree.NodeBase;
using Simulator.actioncommands;
using System.Linq;

namespace BehaviorTree.Actions
{
    class EnemyScore : IActionStrategy
    {
        public bool HandleEnemy(EnemyBlackboard blackboard)
        {
            if(blackboard.LegalActions.Any(a => a is ScorePoints))
            {
                blackboard.ChoosenAction = blackboard.LegalActions.First(a => a is ScorePoints);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool HandleTurret(TurretBlackboard blackboard)
        {
            return false;
        }
    }
}
