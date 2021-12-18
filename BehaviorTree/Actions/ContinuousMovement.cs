using BehaviorTree.NodeBase;
using Simulator;

namespace BehaviorTree.Actions
{
    class ContinuousMovement : IActionStrategy
    {
        public ResultEnum Result { get; set; }
        private readonly IActionStrategy strategy;
        private readonly Direction direction;

        public ContinuousMovement(IActionStrategy strategy, Direction direction)
        {
            this.strategy = strategy;
            this.direction = direction;
        }

        public void HandleEnemy(EnemyBlackboard blackboard)
        {
            strategy.HandleEnemy(blackboard);
            if (strategy.Result == ResultEnum.Failed)
                Result = ResultEnum.Failed;
            else
            {
                if (blackboard.WallDistances[direction] == 1)
                    Result = ResultEnum.Succeeded;
                else
                    Result = ResultEnum.Running;
            }
        }

        public void HandleTurret(TurretBlackboard blackboard)
        {
            Result = ResultEnum.Failed;
        }

        public override string ToString()
        {
            return $"Continue{strategy}";
        }
    }
}
