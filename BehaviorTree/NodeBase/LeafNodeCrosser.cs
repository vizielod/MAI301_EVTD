using System;
using System.Collections.Generic;
using System.Linq;

namespace BehaviorTree.NodeBase
{
    class LeafNodeCrosser: INodeCrosser
    {
        private readonly ParentNode nodeA;
        private readonly ParentNode nodeB;
        private readonly Random random = new Random();

        public LeafNodeCrosser(ParentNode nodeA, ParentNode nodeB)
        {
            this.nodeA = nodeA;
            this.nodeB = nodeB;
        }

        public void Cross()
        {
            int sizeA = nodeA.Count(false);
            int sizeB = nodeB.Count(false);

            // Find random id for NodeA and NodeB
            int indexA = random.Next(sizeA);
            int indexB = random.Next(sizeB);

            // Extract node at randomId from Node A and B
            var resultA = Get(ref indexA, nodeA).Value;
            int localIndexA = resultA.parent.IndexOf(resultA.child);
            resultA.parent.RemoveChild(resultA.child);

            var resultB = Get(ref indexB, nodeB).Value;
            int localIndexB = resultB.parent.IndexOf(resultB.child);
            resultB.parent.RemoveChild(resultB.child);

            // Insert node from A to B and vice versa
            resultA.parent.Insert(localIndexA, resultB.child);
            resultB.parent.Insert(localIndexB, resultA.child);

        }

        public (ParentNode parent, Node child)? Get(ref int index, ParentNode parent)
        {
            foreach (var node in ((ParentNodeController)parent.GetControl()).subnodes)
            {
                if (node is ParentNode parentNode)
                {
                    var result = Get(ref index, parentNode);
                    if (result != null)
                        return result;
                }
                else
                {
                    if (index == 0)
                        return (parent, node);
                    index--;
                }
            }

            return null;
        }
    }
}
