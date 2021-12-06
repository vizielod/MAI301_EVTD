using BehaviorTree.NodeBase;
using System.Linq;

namespace BehaviorTree.FlowControllNodes
{
    class Sequence:ParentNode
    {
        public Sequence()
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
