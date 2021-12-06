using BehaviorTree.ActionNodes;
using BehaviorTree.Agents;
using BehaviorTree.ConditionalNodes;
using BehaviorTree.FlowControllNodes;
using BehaviorTree.NodeBase;
using Simulator;
using System;
using System.Collections.Generic;

namespace BehaviorTree
{
    class SimpleEnemyAgent : IEnemyAgent
    {
        public (int x, int y) InitialPosition { get; }
        public int SpawnRound { get; }

        EnemyBlackboard bb;
        private readonly int maxHealth;

        public bool IsActive => Health > 0;

        public int Health { get; set; }

        public bool IsEnemy => true;

        public float HealthRatio => Health / maxHealth;

        public SimpleEnemyAgent((int x, int y) initialPosition, int spawnRound) 
        {
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

            bb.ForwardPosition = state.SuggestPosition(this);
            bb.CurrentPosition = state.PositionOf(this);
            
            Selector move = new Selector();

            Sequence repeatSeq = new Sequence();
            repeatSeq.AddChildren(new CanRepeatLastMove(bb));
            repeatSeq.AddChildren(new RepeatPreviousAction( bb));
            move.AddChildren(repeatSeq);

            Sequence moveSouth = new Sequence();
            moveSouth.AddChildren(new CanMoveSouth(bb));
            moveSouth.AddChildren(new MoveSouth(bb));
            move.AddChildren(moveSouth);

            Sequence moveEast = new Sequence();
            moveEast.AddChildren(new CanMoveEast(bb));
            moveEast.AddChildren(new MoveEast(bb));
            move.AddChildren(moveEast);

            Sequence moveWest = new Sequence();
            moveWest.AddChildren(new CanMoveWest(bb));
            moveWest.AddChildren(new MoveWest(bb));
            move.AddChildren(moveWest);


            Sequence moveNorth = new Sequence();
            moveNorth.AddChildren(new CanMoveNorth(bb));
            moveNorth.AddChildren(new MoveNorth(bb));
            move.AddChildren(moveNorth);

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
            Health -= v;
        }

        public void Heal(int v)
        {
            Health += v;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
