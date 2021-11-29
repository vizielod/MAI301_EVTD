using BehaviorTree.NodeBase;
using System.Linq;

namespace BehaviorTree.FlowControllNodes
{
    public class Selector: ParentNode
    {
        public Selector()
        {}

        /**
        * Chooses the new task to update.
        * @return The new task, or null
        * if none was found
        */
        public Node ChooseNewNode()
        {
            Node node = null;
            bool found = false;
            int curPos =
            controller.subnodes.
            IndexOf(controller.currentNode);
            while (!found)
            {
                if (curPos ==
                (controller.subnodes.Count - 1))
                {
                    found = true;
                    node = null;
                    break;
                }
                curPos++;
                node = controller.subnodes.
                ElementAt(curPos);
                if (node.CheckConditions())
                {
                    found = true;
                }
            }
            return node;
        }

        public override void ChildFailed()
        {
            controller.currentNode = ChooseNewNode();
            if (controller.currentNode == null)
            {
                controller.FinishWithFailure();
            }
        }

        public override void ChildSucceeded()
        {
            controller.FinishWithSuccess();
        }

        public override void AddChildren(Node node)
        {
            controller.AddNode(node);
        }
    }
}
