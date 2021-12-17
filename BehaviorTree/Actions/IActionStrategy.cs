using BehaviorTree.NodeBase;

namespace BehaviorTree.Actions
{
    public enum ResultEnum
    {
        Succeeded,
        Failed,
        Running
    }
    internal interface IActionStrategy
    {
        ResultEnum Result { get; }
        void HandleEnemy(EnemyBlackboard blackboard);
        void HandleTurret(TurretBlackboard blackboard);
    }
}