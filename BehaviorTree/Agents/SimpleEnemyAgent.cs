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
            bb = new Blackboard(null, null);
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
            
            Selector move = new Selector("Selector", bb);
            

            move.AddChildren(new RepeatPreviousAction("Repeat", bb));
            move.AddChildren(new MoveSouth("MoveSouth", bb));
            move.AddChildren(new MoveEast("MoveEast", bb));
            move.AddChildren(new MoveWest("MoveWest", bb));
            move.AddChildren(new MoveNorth("MoveNorth", bb));

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
