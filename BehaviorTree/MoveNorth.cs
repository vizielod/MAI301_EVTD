using System;
using System.Linq;
using Simulator.actioncommands;

namespace BehaviorTree
{
    class MoveNorth : LeafNode
    {
        public MoveNorth(string name, Blackboard blackboard) : base(name, blackboard)
        { }

        public override bool CheckConditions()
        {
            return blackboard.LegalActions != null && blackboard.LegalActions.Any();
        }

        public override void DoAction()
        {
            LogTask("Doing action");
           
            if (blackboard.LegalActions.Any(a => a is GoNorth))
            {
                blackboard.ChoosenAction = blackboard.LegalActions.First(a => a is GoNorth);
                controller.FinishWithSuccess();
            }
            else
            {
                controller.FinishWithFailure();
            }
            
        }
    }
}
