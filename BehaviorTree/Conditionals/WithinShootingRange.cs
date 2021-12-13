using BehaviorTree.NodeBase;
using System;

namespace BehaviorTree.Conditionals
{
    class WithinShootingRange : IConditionStrategy
    {
        public bool HandleEnemy(EnemyBlackboard blackboard)
        {
            if (blackboard.ClosestTurret != null && blackboard.CurrentPosition != null && blackboard.ClosestTurretPosition != null)
            {
                (int x, int y)? turretPos = blackboard.ClosestTurretPosition;
                (int x, int y)? currentPosition = blackboard.CurrentPosition;
                (int x, int y) p = (currentPosition.Value.x - turretPos.Value.x, currentPosition.Value.y - turretPos.Value.y);
                // It might be better to put the Range on the IAgent interface instead of this cast, it would be useful if we have mutiple turret types
                TurretAgent t = blackboard.ClosestTurret as TurretAgent;
                return Math.Max(Math.Abs(p.x), Math.Abs(p.y)) <= t.Range;
            }
            else
            {
                return false;
            }
        }

        public bool HandleTurret(TurretBlackboard blackboard)
        {
            return false;
        }
    }
}
