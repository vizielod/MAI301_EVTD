using BehaviorTree.NodeBase;
using Simulator.actioncommands;
using System.Linq;

namespace BehaviorTree.Actions
{
    class EnemyScore : IActionStrategy
    {
        public ResultEnum Result { get; set; }

        public void HandleEnemy(EnemyBlackboard blackboard)
        {
            if(blackboard.LegalActions.Any(a => a is ScorePoints))
            {
                blackboard.ChoosenAction = blackboard.LegalActions.First(a => a is ScorePoints);
                Result = ResultEnum.Succeeded;
            }
            else
            {
                Result = ResultEnum.Failed;
            }
        }

        public void HandleTurret(TurretBlackboard blackboard)
        {
            Result = ResultEnum.Failed;
        }
    }
}
