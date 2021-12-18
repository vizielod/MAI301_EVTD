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
        GoNorth,
        GoSouth,
        GoEast,
        GoWest,
        GoNowhere,
        RepeatAction,
        ContinueEast,
        ContinueWest,
        ContinueNorth,
        ContinueSouth
    }

    public enum CompositeType
    {
        Selector,
        Sequence
    }

    public enum ConditionType
    {
        CanRepeat,
        AttackedFromEast,
        AttackedFromWest,
        AttackedFromSouth,
        AttackedFromNorth,
        WithinShootingRange,
        IsNorthOptimal,
        IsSouthOptimal,
        IsEastOptimal,
        IsWestOptimal,
        Hurt
    }
    public class AgentBuilder
    {
        EnemyBlackboard blackboard;
        ParentNode currentNode;
        int spawnRound;
        (int x, int y)? initialPosition;
        Random rand = new Random();

        internal ParentNode RootNode { get; set; }

        public AgentBuilder(CompositeType rootType) : this(MakeCompositeNode(rootType))
        {
        }

        internal AgentBuilder(ParentNode root)
        {
            blackboard = new EnemyBlackboard();
            RootNode = currentNode = root;
            spawnRound = 0;
        }

        public AgentBuilder AddAlternateCompositeToRoot()
        {
            currentNode = RootNode;
            return AddAlternateComposite();
        }

        public AgentBuilder AddAlternateComposite()
        {
            return AddCompositeNode(GetAlternative(currentNode));
        }

        private CompositeType GetAlternative(ParentNode parent)
        {
            if (parent is Selector)
                return CompositeType.Sequence;
            else
                return CompositeType.Selector;
        }

        public AgentBuilder AddNodesToRoot(CompositeType compositeType, ConditionType conditionType, ActionType actionType) 
        {
            currentNode = RootNode;
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

        private static IActionStrategy MakeActionStrategy(ActionType type)
        {
            switch (type)
            {
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
                case ActionType.RepeatAction:
                    return new RepeatPreviousAction();
                case ActionType.ContinueEast:
                    return new ContinuousMovement(new MoveEast(), Simulator.Direction.East);
                case ActionType.ContinueWest:
                    return new ContinuousMovement(new MoveWest(), Simulator.Direction.West);
                case ActionType.ContinueNorth:
                    return new ContinuousMovement(new MoveNorth(), Simulator.Direction.North);
                case ActionType.ContinueSouth:
                    return new ContinuousMovement(new MoveSouth(), Simulator.Direction.South);
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

        private static IConditionStrategy MakeConditionStrategy(ConditionType type)
        {
            switch (type)
            {
                case ConditionType.CanRepeat:
                    return new CanRepeatLastMove();
                case ConditionType.AttackedFromEast:
                    return new AttackedFromEast();
                case ConditionType.AttackedFromWest:
                    return new AttackedFromWest();
                case ConditionType.AttackedFromSouth:
                    return new AttackedFromSouth();
                case ConditionType.AttackedFromNorth:
                    return new AttackedFromNorth();
                case ConditionType.WithinShootingRange:
                    return new WithinShootingRange();
                case ConditionType.IsNorthOptimal:
                    return new IsNorthOptimal();
                case ConditionType.IsSouthOptimal:
                    return new IsSouthOptimal();
                case ConditionType.IsEastOptimal:
                    return new IsEastOptimal();
                case ConditionType.IsWestOptimal:
                    return new IsWestOptimal();
                case ConditionType.Hurt:
                    return new HealthBelow(0.5f);
            }
            return null;
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

        private static ParentNode MakeCompositeNode(CompositeType compositeType)
        {
            switch (compositeType)
            {
                case CompositeType.Sequence:
                    return new Sequence();
                default:
                    return new Selector();
            }
        }

        public AgentBuilder AddCompositeNode(CompositeType compositeType) 
        {
            ParentNode node = MakeCompositeNode(compositeType);
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
            try
            {
                if (rand.Next(1) == 1)
                {
                    var candidate = RootNode.GetAllLeafNodes().Where(n => n is ActionNode).Cast<ActionNode>().ToList().Random();
                    candidate.Strategy = MakeActionStrategy(RandomSelect.Random<ActionType>());
                }
                else
                {
                    var candidate = RootNode.GetAllLeafNodes().Where(n => n is ConditionalNode).Cast<ConditionalNode>().ToList().Random();
                    candidate.Strategy = MakeConditionStrategy(RandomSelect.Random<ConditionType>());
                }
            }
            catch (Exception ex)
            {

            }
            
        }

        public IAdaptiveEnemy BuildAgent()
        {
            if(initialPosition == null)
                throw new ArgumentNullException(nameof(initialPosition));

            AdaptiveAgent agent = new AdaptiveAgent(initialPosition.Value, spawnRound, blackboard, RootNode);
            return agent;
        }
    }
}
