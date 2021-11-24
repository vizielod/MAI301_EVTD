using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviorTree
{
    class ForceSuccess : DecoratorNode
    {
        public ForceSuccess(string name, Blackboard blackboard, Node node) : base(name, blackboard, node)
        {
        }

        public override void DoAction()
        {
            node.DoAction();

            GetControl().FinishWithSuccess();
        }
    }
}
