using System.Linq;
using BehaviorTree.NodeBase;
using Simulator.actioncommands;

namespace BehaviorTree.Actions
{
    class MoveNorth : IActionStrategy
    {
        public ResultEnum Result { get; set; }

        public void HandleEnemy(EnemyBlackboard blackboard)
        {
            var action = blackboard.LegalActions.FirstOrDefault(a => a is GoNorth);

            if (action != null)
            {
                blackboard.ChoosenAction = action;
                Result = ResultEnum.Succeeded;
            }
            Result = ResultEnum.Failed;
        }

        public void HandleTurret(TurretBlackboard blackboard)
        {
            Result = ResultEnum.Failed;
        }
    }
}
