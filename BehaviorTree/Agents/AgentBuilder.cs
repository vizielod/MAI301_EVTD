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
        Random rand = new Random();
        public AgentBuilder()
        {
            blackboard = new EnemyBlackboard();
            rootNode = currentNode = new Selector();
            spawnRound = 0;
        }

        public AgentBuilder AddNodesToRoot(CompositeType compositeType, ConditionType conditionType, ActionType actionType) 
        {
            currentNode = rootNode;
            AddCompositeNode(compositeType);
            AddConditionNode(conditionType);
            AddActionNode(actionType);
            return this;
        }

        public AgentBuilder AddActionNode(ActionType type)
        {
            AddLeafNode(
                new ActionNode(
                    MakeActionStrategy(type)));

            return this;
        }

        private IActionStrategy MakeActionStrategy(ActionType type)
        {
            switch (type)
            {
                case ActionType.Forward:
                    return new MoveForward();
                case ActionType.GoNorth:
                    return new MoveNorth();
                case ActionType.GoSouth:
                    return new MoveSouth();
                case ActionType.GoEast:
                    return new MoveEast();
                case ActionType.GoWest:
                    return new MoveWest();
                case ActionType.GoNowhere:
                    return new Wait();
                case ActionType.Score:
                    return new EnemyScore();
                case ActionType.RepeatAction:
                    return new RepeatPreviousAction();
            }
            return null;
        }

        public AgentBuilder AddConditionNode(ConditionType type)
        {
            AddLeafNode(
                new ConditionalNode(
                    MakeConditionStrategy(type)));

            return this;
        }

        private IConditionStrategy MakeConditionStrategy(ConditionType type)
        {
            switch (type)
            {
                case ConditionType.CanGoSouth:
                    return new CanMoveSouth();
                case ConditionType.CanGoNorth:
                    return new CanMoveNorth();
                case ConditionType.CanGoWest:
                    return new CanMoveWest();
                case ConditionType.CanGoEast:
                    return new CanMoveEast();
                case ConditionType.CanRepeat:
                    return new CanRepeatLastMove();
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
            return null;
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

        public AgentBuilder AddCompositeNode(CompositeType compositeType) 
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
            
            if (rand.Next(1)==1)
            {
                var candidate = newRoot.GetAllLeafNodes().Where(n => n is ActionNode).Cast<ActionNode>().Random();
                candidate.Strategy = MakeActionStrategy(RandomSelect.Random<ActionType>());
            }
            else
            {
                var candidate = newRoot.GetAllLeafNodes().Where(n => n is ConditionalNode).Cast<ConditionalNode>().Random();
                candidate.Strategy = MakeConditionStrategy(RandomSelect.Random<ConditionType>());
            }

            rootNode = newRoot;
        }

        public IAdaptiveEnemy BuildAgent()
        {
            if(initialPosition == null)
                throw new ArgumentNullException(nameof(initialPosition));

            AdaptiveAgent agent = new AdaptiveAgent(initialPosition.Value, spawnRound, blackboard, rootNode);
            return agent;
        }

        public void Cross(AgentBuilder dad)
        {
            List<Node> flattenedMom = rootNode.Flatten().ToList();

            IEnumerable<Node> flattenedDad = dad.rootNode.Flatten();

        }
    }
}
