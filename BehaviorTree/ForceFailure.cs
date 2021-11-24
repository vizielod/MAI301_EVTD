using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviorTree
{
    class ForceFailure : DecoratorNode
    {
        public ForceFailure(Blackboard blackboard, Node node) : base( blackboard, node)
        {
        }

        public override void DoAction()
        {
            node.DoAction();

            GetControl().FinishWithFailure();
        }
    }
}
