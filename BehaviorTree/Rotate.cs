using Simulator.actioncommands;
using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviorTree
{
    class Rotate: LeafNode
    {
        public Rotate(string name, Blackboard blackboard) : base(name, blackboard)
        { }

        public override bool CheckConditions()
        {
            return true;
        }

        public override void DoAction()
        {
            blackboard.ChoosenAction = new Turn();
            controller.FinishWithSuccess();
        }
    }
}

