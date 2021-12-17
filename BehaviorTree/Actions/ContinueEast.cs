using System;
using BehaviorTree.Conditionals;
using BehaviorTree.NodeBase;
using Simulator;

namespace BehaviorTree.Actions
{
    class ContinueEast : IActionStrategy
    {
        public ResultEnum Result { get; set; }
        MoveEast moveEast = new MoveEast();

        public void HandleEnemy(EnemyBlackboard blackboard)
        {
            moveEast.HandleEnemy(blackboard);
            if (moveEast.Result == ResultEnum.Failed)
                Result = ResultEnum.Failed;
            else
            {
                if (blackboard.WallDistances[Direction.East] == 1)
                    Result = ResultEnum.Succeeded;
                else
                    Result = ResultEnum.Running;
            }
        }

        public void HandleTurret(TurretBlackboard blackboard)
        {
            Result = ResultEnum.Failed;
        }
    }
}
