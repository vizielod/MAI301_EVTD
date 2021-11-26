using Simulator;
using System.Collections.Generic;

namespace BehaviorTree.NodeBase
{
    public class Blackboard
    {
        // Enemy
        public IEnumerable<IAction> LegalActions { get; set; }
        public (int x, int y)? ForwardPosition { get; set; }
        public (int x, int y)? CurrentPosition { get; set; }
        public (int x, int y)? ClosestTurretPosition { get; set; }
        public IAgent ClosestTurret { get; set; }

        // Common
        public IAction ChoosenAction { get; set; }
        public IAction PreviousAction { get; set; }

        // Turret
        public IAgent ClosestEnemy { get; set; }
        public IAgent PreviousTargetEnemy { get; set; }
        public bool IsEnemyInRange { get; set; }
        public int Damage { get; set; }

        public Blackboard() 
        {

            LegalActions = null;
            PreviousAction = null;
            ChoosenAction = null;
            ClosestEnemy = null;
            PreviousTargetEnemy = null;
            IsEnemyInRange = false;
            Damage = 0;
            ForwardPosition = null;
            CurrentPosition = null;
            ClosestTurret = null;
            ClosestTurretPosition = null;
        }
    }
}
