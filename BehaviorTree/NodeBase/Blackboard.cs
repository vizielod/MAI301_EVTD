using Simulator;
using System.Collections.Generic;

namespace BehaviorTree
{
    public class Blackboard
    {
        public IEnumerable<IAction> legalActions;
        public IAction choosenAction;
        public IAction previousAction;

        public Blackboard(IEnumerable<IAction> legalActions, IAction previousAction) 
        {
            this.legalActions = legalActions;
            this.previousAction = previousAction;
            this.choosenAction = null;
        }
    }
}
