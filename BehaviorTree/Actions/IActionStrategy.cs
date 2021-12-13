using BehaviorTree.NodeBase;

namespace BehaviorTree.Actions
{
    internal interface IActionStrategy
    {
        bool HandleEnemy(EnemyBlackboard blackboard);
        bool HandleTurret(TurretBlackboard blackboard);
    }
}