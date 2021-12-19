using System;
using System.Collections.Generic;

namespace BehaviorTree
{
    public enum NodeType
    {
        Composite,
        Conditional,
        Action,
        Decorator
    }

    public interface INodeInfo
    {
        Guid ID { get; }
        string Name { get; }
        NodeType Type { get; }
        Guid? Parent { get; }
    }

    public interface ITraverser
    {
        IEnumerable<(int, IEnumerable<INodeInfo>)> GenerateTreeNodes();
        int CountWidth();
        int CountHeight();
    }
}
