using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviorTree
{
    class ForceSuccess : DecoratorNode
    {
        public ForceSuccess(Blackboard blackboard, Node node) : base( blackboard, node)
        {
        }

        public override void DoAction()
        {
            node.DoAction();

            GetControl().FinishWithSuccess();
        }
    }
}
