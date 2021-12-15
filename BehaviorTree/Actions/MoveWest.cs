using System.Linq;
using BehaviorTree.NodeBase;
using Simulator.actioncommands;

namespace BehaviorTree.Actions
{
    class MoveWest : IActionStrategy
    {
        public bool HandleEnemy(EnemyBlackboard blackboard)
        {
            var action = blackboard.LegalActions.FirstOrDefault(a => a is GoWest);

            if (action != null)
            {
                blackboard.ChoosenAction = action;
                return true;
            }
            return false;
        }

        public bool HandleTurret(TurretBlackboard blackboard)
        {
            return false;
        }
    }
}
