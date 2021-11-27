using BehaviorTree.ActionNodes;
using BehaviorTree.Agents;
using BehaviorTree.FlowControllNodes;
using BehaviorTree.NodeBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviorTree.Agents
{
    public enum ActionType 
    {
        Forward
    }
    public class AgentBuilder
    {
        EnemyBlackboard blackboard;
        ParentNode rootNode;
        ParentNode currentNode;
        int spawnRound;
        (int x, int y)? initialPosition;
        public AgentBuilder()
        {
            blackboard = new EnemyBlackboard();
            rootNode = new Selector();
            currentNode = rootNode;
            spawnRound = 0;
        }

        public AgentBuilder AddActionNode(ActionType type)
        {
            switch (type)
            {
                case ActionType.Forward:
                    AddLeafNode(new MoveForward(blackboard));
                    break;
                default:
                    break;
            }

            return this;
        }

        public AgentBuilder AddSelector()
        {
            Selector selector = new Selector();

            AddCompositeNode(selector);
            return this;
        }

        public AgentBuilder SetInitialPosition(int x, int y)
        {
            initialPosition = (x,y);
            return this;
        }

        public AgentBuilder SetSpawnRound(int spawnRound)
        {
            this.spawnRound = spawnRound;
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
        public IAdaptiveAgent BuildAgent()
        {
            if(initialPosition == null)
                throw new ArgumentNullException(nameof(initialPosition));

            AdaptiveAgent agent = new AdaptiveAgent(initialPosition.Value,spawnRound,blackboard,rootNode);
            return agent;
        }
    }
}
