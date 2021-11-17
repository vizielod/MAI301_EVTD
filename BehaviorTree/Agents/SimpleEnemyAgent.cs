using Simulator;
using System;
using System.Collections.Generic;

namespace BehaviorTree
{
    public class SimpleEnemyAgent : IAgent
    {
        public (int x, int y) InitialPosition { get; }
        public int SpawnRound { get; };

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
         
           

            Selector move = new Selector("Selector", bb);
            ((ParentNodeController)move.GetControl()).
               AddNode(new RepeatPrevActionDecorator("Repeat",bb, new MoveSouth(
               "MoveSouth", bb)));
            ((ParentNodeController)move.GetControl()).
                AddNode(new MoveEast(
                "MoveEast", bb));
            ((ParentNodeController)move.GetControl()).
                AddNode(new MoveWest(
                "MoveWest", bb));
            ((ParentNodeController)move.GetControl()).
                AddNode(new MoveNorth(
                "MoveNorth", bb));

            ((ParentNodeController)move.GetControl()).SafeStart();

            while (!((ParentNodeController)move.GetControl()).Finished()) 
            {
                move.DoAction();
            }

            ((ParentNodeController)move.GetControl()).SafeEnd();

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
