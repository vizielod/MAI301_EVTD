using BehaviorTree.NodeBase;

namespace BehaviorTree.Actions
{
    class RepeatPreviousAction : IActionStrategy
    {
        public ResultEnum Result { get; set; }

        public void HandleEnemy(EnemyBlackboard blackboard)
        {
            blackboard.ChoosenAction = blackboard.PreviousAction;
            Result = ResultEnum.Succeeded;
        }

        public void HandleTurret(TurretBlackboard blackboard)
        {
            Result = ResultEnum.Failed;
        }
        public override string ToString()
        {
            return "Repeat";
        }
    }
}
