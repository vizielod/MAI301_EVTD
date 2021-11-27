using BehaviorTree.Agents;
using BehaviorTree.FlowControllNodes;
using BehaviorTree.NodeBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviorTree
{
    public abstract class AgentBuilder
    {
        protected AdaptiveAgent agent;
        EnemyBlackboard blackboard;
        ParentNode rootNode;
        ParentNode currentNode;
        public AgentBuilder()
        {
            blackboard = new EnemyBlackboard();
            rootNode = new Selector();
            currentNode = rootNode;
        }
        // Gets vehicle instance
        public AdaptiveAgent Agent
        {
            get { return agent; }
        }
        // Abstract build methods
        public AgentBuilder AddSelector()
        {
            Selector selector = new Selector();

            AddCompositeNode(selector);
            return this;
        }

        public AgentBuilder AddSequence()
        {
            Sequence sequence = new Sequence();

            AddCompositeNode(sequence);
            return this;
        }
        AgentBuilder AddCompositeNode(ParentNode node) 
        {
            currentNode.AddChildren(node);
            currentNode = node;
            return this;
        }

        AgentBuilder AddLeafNode(Node node)
        {
            currentNode.AddChildren(node);
            return this;
        }
        public AgentBuilder BuildAgent()
        {
            return this;
        }
    }
}
