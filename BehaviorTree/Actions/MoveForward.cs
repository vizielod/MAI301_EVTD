using BehaviorTree.NodeBase;
using Simulator.actioncommands;

namespace BehaviorTree.Actions
{
    class MoveForward : IActionStrategy
    {
        public ResultEnum Result { get; set; }

        public void HandleEnemy(EnemyBlackboard blackboard)
        {
            if (blackboard.ProgressiveAction is Idle)
                Result = ResultEnum.Failed;

            blackboard.ChoosenAction = blackboard.ProgressiveAction;
            Result = ResultEnum.Succeeded;
        }

        public void HandleTurret(TurretBlackboard blackboard)
        {
            Result = ResultEnum.Failed;
        }
    }
}
