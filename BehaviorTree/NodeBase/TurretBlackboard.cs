using Simulator;
using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviorTree.NodeBase
{
    class TurretBlackboard:Blackboard
    {
        public TurretBlackboard()
        {
            Reset();
        }

        // Turret
        public IAgent ClosestEnemy { get; set; }
        public IAgent PreviousTargetEnemy { get; set; }
        public bool IsEnemyInRange { get; set; }
        public int Damage { get; set; }

        public override void AcceptVisitor(LeafNode visitor)
        {
            visitor.HandleTurret(this);
        }

        public override void Reset()
        {
            base.Reset();
            ClosestEnemy = null;
            PreviousTargetEnemy = null;
            IsEnemyInRange = false;
        }
    }
}
