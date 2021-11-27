using BehaviorTree.NodeBase;
using Simulator;
using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviorTree.Agents
{
    public class AdaptiveAgent: IAgent
    {
        public (int x, int y) InitialPosition { get; }
        public int SpawnRound { get; }

        EnemyBlackboard bb;
        Node rootNode;
        public int health;

        public bool IsActive => health > 0;

        public AdaptiveAgent((int x, int y) initialPosition, int spawnRound, EnemyBlackboard bb, Node rootNode)
        {
            this.InitialPosition = initialPosition;
            this.bb = bb;
            this.rootNode = rootNode;
            health = 10;
            this.SpawnRound = spawnRound;
        }

        public IAction PickAction(IState state)
        {
            IEnumerable<IAction> actions = state.GetLegalActionGenerator(this).Generate();

            bb.LegalActions = actions;
            bb.ChoosenAction = null;

            bb.ForwardPosition = state.SuggestPosition(this);
            bb.CurrentPosition = state.PositionOf(this);

           
            return bb.ChoosenAction;
        }

        public void Damage(int v)
        {
            health -= v;
        }

        public void Heal(int v)
        {
            health += v;
        }

        public void Reset() 
        {
            bb.Reset();
        }
    }
}
