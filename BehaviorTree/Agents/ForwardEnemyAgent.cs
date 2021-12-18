using System.Collections.Generic;
using BehaviorTree.Actions;
using BehaviorTree.Agents;
using BehaviorTree.FlowControllNodes;
using BehaviorTree.NodeBase;
using Simulator;

namespace BehaviorTree
{
    public class ForwardEnemyAgent : IEnemyAgent
    {
        public (int x, int y) InitialPosition { get; }
        public int SpawnRound { get; }

        EnemyBlackboard bb;
        private int maxHealth;

        public int Health { get; set; }

        public bool IsActive => Health > 0;

        public bool IsEnemy => true;

        public float HealthRatio => Health / maxHealth;

        private ParentNode root;

        public ForwardEnemyAgent((int x, int y) initialPosition, int spawnRound)
        {
            this.InitialPosition = initialPosition;
            bb = new EnemyBlackboard();
            maxHealth = Health = 10;
            this.SpawnRound = spawnRound;
            root = new Selector();
            root.AddChildren(new ActionNode(new MoveForward()));
        }

        public IAction PickAction(IState state)
        {
            IEnumerable<IAction> actions = state.GetLegalActionGenerator(this).Generate();

            bb.LegalActions = actions;
            bb.ChoosenAction = null;

            bb.ProgressiveAction = state.SuggestedAction(this);
            bb.CurrentPosition = state.PositionOf(this);

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

