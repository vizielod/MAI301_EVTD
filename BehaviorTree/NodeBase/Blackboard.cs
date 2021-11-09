using Simulator;
using System.Collections.Generic;

namespace BehaviorTree
{
    public class Blackboard
    {
        public IEnumerable<IAction> legalActions;
        public IAction choosenAction;
    }
}
