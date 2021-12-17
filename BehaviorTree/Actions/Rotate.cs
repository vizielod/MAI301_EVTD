using BehaviorTree.NodeBase;
using Simulator.actioncommands;

namespace BehaviorTree.Actions
{
    class Rotate : IActionStrategy
    {
        public ResultEnum Result { get; set; }

        public void HandleEnemy(EnemyBlackboard blackboard)
        {
            Result = ResultEnum.Failed;
        }

        public void HandleTurret(TurretBlackboard blackboard)
        {
            blackboard.ChoosenAction = new Turn();
            Result = ResultEnum.Succeeded;
        }
    }
}

