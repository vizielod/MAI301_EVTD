using BehaviorTree.NodeBase;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BehaviorTree
{
    class Traverser : ITraverser
    {
        private readonly ParentNode root;

        public Traverser(ParentNode root)
        {
            this.root = root;
        }

        public int CountWidth()
        {
            int highest = 0;
            var parents = toList(root);
            while (parents.Any())
            {
                var count = DissectCount(parents);
                if (count > highest)
                    highest = count;
                parents = ExtractComposits(parents);
            }

            return highest;
        }

        public int CountHeight()
        {
            return Dive(root) + 1;
        }

        private int Dive(ParentNode parent, int depth = 1)
        {
            int highest = depth;
            foreach (ParentNode children in ((ParentNodeController)parent.GetControl()).subnodes.Where(c => c is ParentNode))
            {
                var d = Dive(children, depth + 1);
                if (d > highest)
                    highest = d;
            }
            return highest;
        }

        private IEnumerable<ParentNode> ExtractComposits(IEnumerable<ParentNode> parents)
        {
            foreach (var parent in parents)
                foreach (ParentNode child in ((ParentNodeController)parent.GetControl()).subnodes.Where(c => c is ParentNode))
                    yield return child;
        }

        private int DissectCount(IEnumerable<ParentNode> parents)
        {
            return parents.Select(p => (ParentNodeController)p.GetControl()).Sum(c => c.subnodes.Count());
        }
        IEnumerable<T> toList<T>(params T[] list) => list;

        public IEnumerable<(int, IEnumerable<INodeInfo>)> GenerateTreeNodes()
        {
            int level = 0;
            var parents = toList(new NodeInfo(root));
            yield return (level, parents);

            while (parents.Any())
            {
                var children = Dissect(parents).ToArray();
                yield return (++level, children);
                parents = children.Where(c => c.Node is ParentNode);
            }
        }

        private IEnumerable<NodeInfo> Dissect(IEnumerable<NodeInfo> parents)
        {
            foreach (var parent in parents)
            {
                var parentNode = (ParentNode)parent.Node;
                foreach (var node in ((ParentNodeController)parentNode.GetControl()).subnodes)
                    yield return new NodeInfo(node, parent.ID);
            }
        }

        class NodeInfo : INodeInfo
        {
            public NodeInfo(Node node, Guid? parent = null)
            {
                ID = Guid.NewGuid();
                Node = node;
                Parent = parent;
            }

            public Guid ID { get; }

            public string Name => Node.ToString();

            public NodeType Type => Node.Type;

            public Guid? Parent { get; }
            public Node Node { get; }
        }
    }
}
