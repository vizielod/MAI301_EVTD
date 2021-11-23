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
        int health;

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
         
            
            Selector move = new Selector("Selector", bb);
            ParentNodeController pnc = (ParentNodeController)move.GetControl();

            pnc.AddNode(new RepeatPreviousAction("Repeat", bb));
            pnc.AddNode(new MoveSouth("MoveSouth", bb));
            pnc.AddNode(new MoveEast("MoveEast", bb));
            pnc.AddNode(new MoveWest("MoveWest", bb));
            pnc.AddNode(new MoveNorth("MoveNorth", bb));
            pnc.SafeStart();

            while (!pnc.Finished()) 
            {
                move.DoAction();
            }

            pnc.SafeEnd();

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
