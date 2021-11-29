﻿using BehaviorTree.ActionNodes;
using BehaviorTree.Agents;
using BehaviorTree.ConditionalNodes;
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

    public enum CompositeType
    {
        Selector,
        Sequence
    }

    public enum ConditionType
    {
        CanGoSouth,
        CanGoNorth,
        CanGoWest,
        CanGoEast,
        CanRepeat,
        IsAttackingTurretEast,
        IsAttackingTurretWest,
        IsAttackingTurretSouth,
        IsAttackingTurretNorth,
        WithinShootingRange
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

        public AgentBuilder AddRootNodes(CompositeType compositeType, ConditionType conditionType, ActionType actionType) 
        {
            currentNode = rootNode;
            AddCompositeNode(compositeType);
            AddConditionNode(conditionType);
            AddActionNode(actionType);
            return this;
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

        public AgentBuilder AddConditionNode(ConditionType type)
        {
            switch (type)
            {
                case ConditionType.CanGoSouth:
                    AddLeafNode(new CanMoveSouth(blackboard));
                    break;
                case ConditionType.CanGoNorth:
                    AddLeafNode(new CanMoveNorth(blackboard));
                    break;
                case ConditionType.CanGoWest:
                    AddLeafNode(new CanMoveWest(blackboard));
                    break;
                case ConditionType.CanGoEast:
                    AddLeafNode(new CanMoveEast(blackboard));
                    break;
                case ConditionType.CanRepeat:
                    AddLeafNode(new CanRepeatLastMove(blackboard));
                    break;
                case ConditionType.IsAttackingTurretEast:
                    AddLeafNode(new IsAttackingTurretEast(blackboard));
                    break;
                case ConditionType.IsAttackingTurretWest:
                    AddLeafNode(new IsAttackingTurretWest(blackboard));
                    break;
                case ConditionType.IsAttackingTurretSouth:
                    AddLeafNode(new IsAttackingTurretSouth(blackboard));
                    break;
                case ConditionType.IsAttackingTurretNorth:
                    AddLeafNode(new IsAttackingTurretNorth(blackboard));
                    break;
                case ConditionType.WithinShootingRange:
                    AddLeafNode(new WithinShootingRange(blackboard));
                    break;
                default:
                    break;
            }

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

        AgentBuilder AddCompositeNode(CompositeType compositeType) 
        {
            ParentNode node;
            switch (compositeType)
            {
                case CompositeType.Sequence:
                    node = new Sequence();
                    break;
                default:
                    node = new Selector();
                    break;
            }

            currentNode.AddChildren(node);
            currentNode = node;
            return this;
        }

        AgentBuilder AddLeafNode(Node node)
        {
            currentNode.AddChildren(node);
            return this;
        }
        public IEnemyAgent BuildAgent()
        {
            if(initialPosition == null)
                throw new ArgumentNullException(nameof(initialPosition));

            AdaptiveAgent agent = new AdaptiveAgent(initialPosition.Value,spawnRound,blackboard,rootNode);
            return agent;
        }
    }
}
