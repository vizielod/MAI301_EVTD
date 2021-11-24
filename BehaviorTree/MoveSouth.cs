using System;
using System.Linq;
using Simulator.actioncommands;

namespace BehaviorTree
{
    class MoveSouth : LeafNode
    {
        public MoveSouth( Blackboard blackboard) : base( blackboard)
        { }

        public override bool CheckConditions()
        {
            return blackboard.LegalActions != null && blackboard.LegalActions.Any();
        }
        public override void DoAction()
        {
           
            if (blackboard.LegalActions.Any(a => a is GoSouth))
            {
                blackboard.ChoosenAction = blackboard.LegalActions.First(a => a is GoSouth);
                controller.FinishWithSuccess();
            }
            else
            {
                controller.FinishWithFailure();
            }
            

        }
    }
}
