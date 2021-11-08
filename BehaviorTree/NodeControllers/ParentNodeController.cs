using System;
using System.Collections.Generic;
using System.Linq;

namespace BehaviorTree
{
    public class ParentNodeController : NodeController
    {
        /**
         * List of nodes that the parent holds
         */
        public List<Node> subnodes;

        /**
         * Current node
         */
        public Node currentNode;

        public ParentNodeController(Node node):base(node)
        {
            this.subnodes = new List<Node>();
            this.currentNode = null;
        }

        /**
         * Adds a node to the subnodes
         */
        public void AddNode(Node node)
        {
            subnodes.Add(node);
        }

        /**
        * Resets the task as if it had
        * just started.
        */
        public new void Reset()
        {
            base.Reset();
            this.currentNode =
            subnodes.First();
        }
    }
}
