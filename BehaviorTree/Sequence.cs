using System;
using System.Linq;

namespace BehaviorTree
{
    public class Sequence:ParentNode
    {
        public Sequence( Blackboard blackboard) : base( blackboard)
        { }

        public override void AddChildren(Node node)
        {
            controller.AddNode(node);
        }

        public override void ChildFailed()
        {
            controller.FinishWithFailure();
        }

        public override void ChildSucceeded()
        {
            int curPos =
            controller.subnodes.
            IndexOf(controller.currentNode);
            if (curPos ==
            (controller.subnodes.Count - 1))
            {
                controller.FinishWithSuccess();
            }
            else
            {
                controller.currentNode =
                controller.subnodes.
                ElementAt(curPos + 1);
                if (!controller.currentNode.CheckConditions())
                {
                    controller.FinishWithFailure();
                }
            }
        }
    }
}
