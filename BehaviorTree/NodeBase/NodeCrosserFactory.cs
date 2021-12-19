using System;
using BehaviorTree.Agents;

namespace BehaviorTree.NodeBase
{
    public class NodeCrosserFactory
    {
        public INodeCrosser Create(AgentBuilder builderA, AgentBuilder builderB)
        {
            return new NodeCrosser(builderA.RootNode, builderB.RootNode);
        }

        public INodeCrosser CreateLeafNode(AgentBuilder builderA, AgentBuilder builderB)
        {
            return new LeafNodeCrosser(builderA.RootNode, builderB.RootNode);
        }
    }
}
