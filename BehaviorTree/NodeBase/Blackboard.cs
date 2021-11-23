using Simulator;
using System.Collections.Generic;

namespace BehaviorTree
{
    public class Blackboard
    {
        // Enemy
        public IEnumerable<IAction> LegalActions { get; set; }

        // Common
        public IAction ChoosenAction { get; set; }
        public IAction PreviousAction { get; set; }

        // Turret
        public IAgent ClosestEnemy { get; set; }
        public IAgent PreviousTargetEnemy { get; set; }
        public bool IsEnemyInRange { get; set; }
        public int Damage { get; set; }

        public Blackboard(IEnumerable<IAction> legalActions, IAction previousAction) 
        {

            LegalActions = legalActions;
            PreviousAction = previousAction;
            ChoosenAction = null;
            ClosestEnemy = null;
            PreviousTargetEnemy = null;
            IsEnemyInRange = false;
            Damage = 0;
        }
    }
}
