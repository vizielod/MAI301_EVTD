using BehaviorTree.NodeBase;

namespace BehaviorTree.Conditionals
{
    internal interface IConditionStrategy
    {
        bool HandleEnemy(EnemyBlackboard blackboard);
        bool HandleTurret(TurretBlackboard blackboard);
    }
}