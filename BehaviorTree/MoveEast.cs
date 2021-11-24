using System;
using System.Linq;
using Simulator.actioncommands;

namespace BehaviorTree
{
    class MoveEast : LeafNode
    {
        public MoveEast(Blackboard blackboard):base( blackboard) 
        { }

        public override bool CheckConditions()
        {
            return blackboard.LegalActions != null && blackboard.LegalActions.Any();
        }

        public override void DoAction()
        {
           
            if (blackboard.LegalActions.Any(a => a is GoEast))
            {
                blackboard.ChoosenAction = blackboard.LegalActions.First(a => a is GoEast);
                controller.FinishWithSuccess();
            }
            else
            {
                controller.FinishWithFailure();
            }
            
        }
    }
}
