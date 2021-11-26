using BehaviorTree.NodeBase;
using System;

namespace BehaviorTree.ConditionalNodes
{
    class CanClosestTurretShootMe:LeafNode
    {
        public CanClosestTurretShootMe(Blackboard blackboard) : base(blackboard)
        {
        }

        public override bool CheckConditions()
        {
            return blackboard.ClosestEnemy != null && blackboard.CurrentPosition != null && blackboard.ClosestTurretPosition != null;
        }

        public override void DoAction()
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
    }
}
