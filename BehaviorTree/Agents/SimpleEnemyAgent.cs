using BehaviorTree.ActionNodes;
using BehaviorTree.ConditionalNodes;
using BehaviorTree.FlowControllNodes;
using BehaviorTree.NodeBase;
using Simulator;
using System;
using System.Collections.Generic;

namespace BehaviorTree
{
    public class SimpleEnemyAgent : IAgent
    {
        public (int x, int y) InitialPosition { get; }
        public int SpawnRound { get; }

        Blackboard bb;
        public int health;

        public bool IsActive => health > 0;

        public SimpleEnemyAgent((int x, int y) initialPosition, int spawnRound) 
        {
            this.InitialPosition = initialPosition;
            bb = new Blackboard();
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
            
            Selector move = new Selector( bb);

            Sequence repeatSeq = new Sequence(bb);
            repeatSeq.AddChildren(new CanRepeatLastMove(bb));
            repeatSeq.AddChildren(new RepeatPreviousAction( bb));
            move.AddChildren(repeatSeq);

            Sequence moveSouth = new Sequence(bb);
            moveSouth.AddChildren(new CanGoSouth(bb));
            moveSouth.AddChildren(new MoveSouth(bb));
            move.AddChildren(moveSouth);

            Sequence moveEast = new Sequence(bb);
            moveEast.AddChildren(new CanMoveEast(bb));
            moveEast.AddChildren(new MoveEast(bb));
            move.AddChildren(moveEast);

            Sequence moveWest = new Sequence(bb);
            moveWest.AddChildren(new CanMoveWest(bb));
            moveWest.AddChildren(new MoveWest(bb));
            move.AddChildren(moveWest);


            Sequence moveNorth = new Sequence(bb);
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
            health -= v;
        }

        public void Heal(int v)
        {
            health += v;
        }
    }
}
