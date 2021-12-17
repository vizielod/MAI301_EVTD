using BehaviorTree.NodeBase;
using Simulator;
using Simulator.actioncommands;
using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviorTree.Agents
{
    class AdaptiveAgent: IAdaptiveEnemy
    {
        public (int x, int y) InitialPosition { get; }
        public int SpawnRound { get; }

        EnemyBlackboard bb;
        Node rootNode;

        public bool IsActive => Health > 0;

        public int Health { get; set; }

        public bool IsEnemy => true;

        public float HealthRatio => Health / maxHealth;

        private int maxHealth;

        public AdaptiveAgent((int x, int y) initialPosition, int spawnRound, EnemyBlackboard bb, Node rootNode)
        {
            this.InitialPosition = initialPosition;
            this.bb = bb;
            this.rootNode = rootNode;
            maxHealth = Health = 10;
            this.SpawnRound = spawnRound;
        }

        public IAction PickAction(IState state)
        {
            IEnumerable<IAction> actions = state.GetLegalActionGenerator(this).Generate();

            bb.LegalActions = actions;
            bb.ChoosenAction = new Idle();
            bb.AttackingTurrets.Clear();

            bb.ProgressiveAction = state.SuggestedAction(this);
            bb.CurrentPosition = state.PositionOf(this);
            bb.WallDistances = state.GetWallDistances(this);
            state.GetClosestTurret(this).Apply(t =>
            {
                bb.ClosestTurret = t;
                bb.ClosestTurretPosition = state.PositionOf(t);
            });

            foreach (var turret in state.GetTurretsAttacking(this))
            {
                bb.AttackingTurrets.Add(turret, state.GetDirection(this, turret));
            }

            state.SuggestedAction(this);

            rootNode.Start();

            while (rootNode.Running())
            {
                rootNode.DoAction(bb);
            }

            rootNode.End();

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

        public void Reset() 
        {
            bb.Reset();
        }

        public IEnemyAgent Clone()
        {
            return new AdaptiveAgent(InitialPosition, SpawnRound, new EnemyBlackboard(), rootNode);
        }

        public AgentBuilder ReverseEngineer()
        {
            var (x, y) = InitialPosition;
            return new AgentBuilder().SetInitialPosition(x,y).SetSpawnRound(SpawnRound).SetRootNode((ParentNode)rootNode.DeepCopy());
        }
    }
}
