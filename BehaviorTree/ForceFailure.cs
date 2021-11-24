using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviorTree
{
    class ForceFailure : DecoratorNode
    {
        public ForceFailure(string name, Blackboard blackboard, Node node) : base(name, blackboard, node)
        {
        }

        public override void DoAction()
        {
            node.DoAction();

            GetControl().FinishWithFailure();
        }
    }
}
