using BehaviorTree.NodeBase;
using System;

namespace BehaviorTree.ConditionalNodes
{
    class WithinShootingRange:LeafNode
    {
        public WithinShootingRange()
        {
        }

        public override bool CheckConditions()
        {
            return true;
        }

        public override void HandleEnemy(EnemyBlackboard blackboard)
        {
            if (blackboard.ClosestTurret != null && blackboard.CurrentPosition != null && blackboard.ClosestTurretPosition != null)
            {
                (int x, int y)? turretPos = blackboard.ClosestTurretPosition;
                (int x, int y)? currentPosition = blackboard.CurrentPosition;
                (int x, int y) p = (currentPosition.Value.x - turretPos.Value.x, currentPosition.Value.y - turretPos.Value.y);
                // It might be better to put the Range on the IAgent interface instead of this cast, it would be useful if we have mutiple turret types
                TurretAgent t = blackboard.ClosestTurret as TurretAgent;
                if (Math.Max(Math.Abs(p.x), Math.Abs(p.y)) <= t.Range)
                {
                    controller.FinishWithSuccess();
                }
                else
                {
                    controller.FinishWithFailure();
                }
            }
            else
            {
                controller.FinishWithFailure();
            }
        }

        public override void HandleTurret(TurretBlackboard blackboard)
        {
            controller.FinishWithFailure();
        }
    }
}
