using BehaviorTree.NodeBase;
using Simulator;
using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviorTree.Agents
{
    class AdaptiveAgent: IEnemyAgent
    {
        public (int x, int y) InitialPosition { get; }
        public int SpawnRound { get; }

        EnemyBlackboard bb;
        Node rootNode;

        public bool IsActive => Health > 0;

        public int Health { get; set; }

        public AdaptiveAgent((int x, int y) initialPosition, int spawnRound, EnemyBlackboard bb, Node rootNode)
        {
            this.InitialPosition = initialPosition;
            this.bb = bb;
            this.rootNode = rootNode;
            Health = 10;
            this.SpawnRound = spawnRound;
        }

        public IAction PickAction(IState state)
        {
            IEnumerable<IAction> actions = state.GetLegalActionGenerator(this).Generate();

            bb.LegalActions = actions;
            bb.ChoosenAction = null;

            bb.ForwardPosition = state.SuggestPosition(this);
            bb.CurrentPosition = state.PositionOf(this);

            rootNode.Start();

            while (rootNode.Running())
            {
                rootNode.DoAction();
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
    }
}
