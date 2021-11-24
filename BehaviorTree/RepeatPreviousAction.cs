﻿using System;
using System.Linq;

namespace BehaviorTree
{
    public class RepeatPreviousAction:LeafNode
    {
        public RepeatPreviousAction(string name, Blackboard bb):base(name,bb)
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
