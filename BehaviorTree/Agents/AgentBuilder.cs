using BehaviorTree.Actions;
using BehaviorTree.Agents;
using BehaviorTree.Conditionals;
using BehaviorTree.FlowControllNodes;
using BehaviorTree.NodeBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorTree.Agents
{

    public enum ActionType 
    {
        Forward,
        GoNorth,
        GoSouth,
        GoEast,
        GoWest,
        GoNowhere,
        RepeatAction,
        Score
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
       /* IsAttackingTurretEast,
        IsAttackingTurretWest,
        IsAttackingTurretSouth,
        IsAttackingTurretNorth,
        WithinShootingRange*/
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
            rootNode = currentNode = new Selector();
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
            IActionStrategy strategy = null;
            switch (type)
            {
                case ActionType.Forward:
                    strategy = new MoveForward();
                    break;
                case ActionType.GoNorth:
                    strategy = new MoveNorth();
                    break;
                case ActionType.GoSouth:
                    strategy = new MoveSouth();
                    break;
                case ActionType.GoEast:
                    strategy = new MoveEast();
                    break;
                case ActionType.GoWest:
                    strategy = new MoveWest();
                    break;
                case ActionType.GoNowhere:
                    strategy = new Wait();
                    break;
                case ActionType.Score:
                    strategy = new EnemyScore();
                    break;
                case ActionType.RepeatAction:
                    strategy = new RepeatPreviousAction();
                    break;
            }
            AddLeafNode(new ActionNode(strategy));

            return this;
        }

        public AgentBuilder AddConditionNode(ConditionType type)
        {
            IConditionStrategy strategy = null;
            switch (type)
            {
                case ConditionType.CanGoSouth:
                    strategy = new CanMoveSouth();
                    break;
                case ConditionType.CanGoNorth:
                    strategy = new CanMoveNorth();
                    break;
                case ConditionType.CanGoWest:
                    strategy = new CanMoveWest();
                    break;
                case ConditionType.CanGoEast:
                    strategy = new CanMoveEast();
                    break;
                case ConditionType.CanRepeat:
                    strategy = new CanRepeatLastMove();
                    break;
                /*case ConditionType.IsAttackingTurretEast:
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
                    break;*/
            }
            AddLeafNode(new ConditionalNode(strategy));

            return this;
        }

        internal AgentBuilder SetRootNode(ParentNode rootNode)
        {
            this.rootNode = rootNode;
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

        public void Mutate()
        {
            var newRoot = (ParentNode) rootNode.DeepCopy();
            var candidate = newRoot.GetAllLeafNodes().Where(n => n is ActionNode).Cast<ActionNode>().Random();
            candidate.Strategy = new MoveEast();
            rootNode = newRoot;
        }

        public IAdaptiveEnemy BuildAgent()
        {
            if(initialPosition == null)
                throw new ArgumentNullException(nameof(initialPosition));

            AdaptiveAgent agent = new AdaptiveAgent(initialPosition.Value, spawnRound, blackboard, rootNode);
            return agent;
        }
    }

    static class RandomHelper
    {
        public static Random rnd = new Random();

        public static T Random<T>(this IEnumerable<T> list)
        {
            return list.ElementAt(rnd.Next(list.Count()));
        }
    }
}
