using Simulator;
using System;
using System.Collections.Generic;

namespace BehaviorTree
{
    class SimpleEnemyAgent : IAgent
    {
        public (int x, int y) InitialPosition => throw new NotImplementedException();

        public SimpleEnemyAgent() { }

        public IAction PickAction(IState state, IAction previousAction)
        {
            IEnumerable<IAction> actions = state.GetLegalActionGenerator(this).Generate();

            Blackboard bb = new Blackboard(actions, previousAction);

                Selector move = new Selector("Selector", bb);
                ((ParentNodeController)move.GetControl()).
                 AddNode(new MoveEast(
                 "MoveEast", bb));
                ((ParentNodeController)move.GetControl()).
                 AddNode(new MoveSouth(
                 "MoveSouth", bb));
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

                return bb.choosenAction;
        }
    }
}
