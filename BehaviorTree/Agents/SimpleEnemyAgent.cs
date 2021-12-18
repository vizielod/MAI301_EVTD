using BehaviorTree.Actions;
using BehaviorTree.Agents;
using BehaviorTree.Conditionals;
using BehaviorTree.FlowControllNodes;
using BehaviorTree.NodeBase;
using Simulator;
using System;
using System.Collections.Generic;

namespace BehaviorTree
{
    public class SimpleEnemyAgent : IEnemyAgent
    {
        public (int x, int y) InitialPosition { get; }
        public int SpawnRound { get; }

        EnemyBlackboard bb;
        private readonly int maxHealth;

        public bool IsActive => Health > 0;

        public int Health { get; set; }

        public bool IsEnemy => true;

        public float HealthRatio => Health / maxHealth;

        Selector root;

        //move.AddChildren(new ConditionalNode(new CanMoveSouth()));
        

        public SimpleEnemyAgent((int x, int y) initialPosition, int spawnRound) 
        {
            root = new Selector();
            root.AddChildren(new ActionNode(new ContinuousMovement(new MoveSouth(), Direction.South)));
            root.AddChildren(new ActionNode(new ContinuousMovement(new MoveEast(), Direction.East)));

            this.InitialPosition = initialPosition;
            bb = new EnemyBlackboard();
            maxHealth = Health = 10;
            this.SpawnRound = spawnRound;
        }

        public IAction PickAction(IState state)
        {
            IEnumerable<IAction> actions = state.GetLegalActionGenerator(this).Generate();

            bb.LegalActions = actions;
            bb.ChoosenAction = null;

            bb.ProgressiveAction = state.SuggestedAction(this);
            bb.CurrentPosition = state.PositionOf(this);
            bb.WallDistances = state.GetWallDistances(this);

            root.Start();

            while (root.Running()) 
            {
                root.DoAction(bb);
            }

            root.End();

            bb.PreviousAction = bb.ChoosenAction;

            return bb.ChoosenAction;
        }

        public void Damage(int v)
        {
            Health -= v;
        }

        public void Heal(int v)
        {
            Health += v;
        }

        public ITraverser GetTree()
        {
            return new Traverser(root);
        }
    }
}
