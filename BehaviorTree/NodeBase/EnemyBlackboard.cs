using Simulator;
using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviorTree.NodeBase
{
    class EnemyBlackboard:Blackboard
    {
        // Enemy
        public IEnumerable<IAction> LegalActions { get; set; }
        public IAction ProgressiveAction { get; set; }
        public (int x, int y)? CurrentPosition { get; set; }
        public (int x, int y)? ClosestTurretPosition { get; set; }
        public IAgent ClosestTurret { get; set; }
        public int Health { get; set; }
        public IEnumerable<IAgent> AttackingTurrets { get; set; }

        public EnemyBlackboard()
        {
            Reset();
        }

        public override void Reset()
        {
            base.Reset();

            LegalActions = null;
            ProgressiveAction = null;
            CurrentPosition = null;
            ClosestTurret = null;
            ClosestTurretPosition = null;
            AttackingTurrets = null;
        }

        public override void AcceptVisitor(LeafNode visitor)
        {
            visitor.HandleEnemy(this);
        }
    }
}
