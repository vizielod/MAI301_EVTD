using Simulator;
using System.Collections.Generic;

namespace BehaviorTree
{
    public class Blackboard
    {
        public IEnumerable<IAction> LegalActions { get; set; }
        public IAction ChoosenAction { get; set; }
        public IAction PreviousAction { get; set; }
        public IAgent ClosestEnemy { get; set; }
        public bool IsEnemyInRange { get; set; }
        public int Damage { get; set; }

        public Blackboard(IEnumerable<IAction> legalActions, IAction previousAction) 
        {

            LegalActions = legalActions;
            PreviousAction = previousAction;
            ChoosenAction = null;
            ChoosenAction = null;
            IsEnemyInRange = false;
            Damage = 0;
        }
    }
}
