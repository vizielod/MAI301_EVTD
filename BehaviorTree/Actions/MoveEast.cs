using System.Linq;
using BehaviorTree.NodeBase;
using Simulator.actioncommands;

namespace BehaviorTree.Actions
{
    class MoveEast : IActionStrategy
    {
        public ResultEnum Result { get; set; }

        public void HandleEnemy(EnemyBlackboard blackboard)
        {
            var action = blackboard.LegalActions.FirstOrDefault(a => a is GoEast);

            if (action != null)
            {
                blackboard.ChoosenAction = action;
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

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}
