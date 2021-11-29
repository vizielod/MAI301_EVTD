using System.Collections.Generic;
using BehaviorTree.ActionNodes;
using BehaviorTree.FlowControllNodes;
using BehaviorTree.NodeBase;
using Simulator;

namespace BehaviorTree
{
    public class ForwardEnemyAgent : IAgent
    {
        public (int x, int y) InitialPosition { get; }
        public int SpawnRound { get; }

        EnemyBlackboard bb;
        int health;

        public bool IsActive => health > 0;

        public ForwardEnemyAgent((int x, int y) initialPosition, int spawnRound)
        {
            this.InitialPosition = initialPosition;
            bb = new EnemyBlackboard();
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

            Selector move = new Selector( );
            move.AddChildren(new MoveForward( bb));

            move.Start();

            while (move.Running())
            {
                move.DoAction();
            }

            move.End();

            bb.PreviousAction = bb.ChoosenAction;

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
    }
}

