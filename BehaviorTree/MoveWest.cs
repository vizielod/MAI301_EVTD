using System;
using System.Linq;
using Simulator.actioncommands;

namespace BehaviorTree
{
    class MoveWest : LeafNode
    {
        public MoveWest(string name, Blackboard blackboard) : base(name, blackboard)
        { }

        public override bool CheckConditions()
        {
            return blackboard.LegalActions != null && blackboard.LegalActions.Any();
        }

        public override void DoAction()
        {
            LogTask("Doing action");
            
            if (blackboard.LegalActions.Any(a => a is GoWest))
            {
                blackboard.ChoosenAction = blackboard.LegalActions.First(a => a is GoWest);
                controller.FinishWithSuccess();
            }
            else
            {
                controller.FinishWithFailure();
            }
            
        }
    }
}
