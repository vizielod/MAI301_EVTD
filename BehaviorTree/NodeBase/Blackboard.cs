using Simulator;
using System.Collections.Generic;

namespace BehaviorTree.NodeBase
{
    public class Blackboard
    {
        

        // Common
        public IAction ChoosenAction { get; set; }
        public IAction PreviousAction { get; set; }

        public Blackboard() 
        {
            Reset();
            
        }

        public virtual void Reset() 
        {
            PreviousAction = null;
            ChoosenAction = null;
           
            
        }
    }
}
