using BehaviorTree.NodeBase;
using System.Linq;

namespace BehaviorTree.FlowControllNodes
{
    class Sequence : ParentNode
    {
        public Sequence()
        { }

        public override string ToString()
        {
            return GetType().Name;
        }

        public override void ChildFailed()
        {
            controller.FinishWithFailure();
        }

        public override void ChildIsRunning()
        {
            controller.FinishWithRunning();
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

        public override Node DeepCopy()
        {
            var sequence = new Sequence();
            foreach (var child in controller.subnodes)
            {
                sequence.AddChildren(child.DeepCopy());
            }
            return sequence;
        }
    }
}
