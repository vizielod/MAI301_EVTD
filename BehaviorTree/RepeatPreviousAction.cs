using System;
using System.Linq;

namespace BehaviorTree
{
    public class RepeatPreviousAction:LeafNode
    {
        public RepeatPreviousAction( Blackboard bb):base(bb)
        {
        }

        public override bool CheckConditions()
        {
            return blackboard.LegalActions.Contains(blackboard.PreviousAction);
        }

        public override void DoAction()
        {
            if (CheckConditions())
            {
                blackboard.ChoosenAction = blackboard.PreviousAction;
                controller.FinishWithSuccess();
            }
            else
            {
                controller.FinishWithFailure();
            }
        }
    }
}
