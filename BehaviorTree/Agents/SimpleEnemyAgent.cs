﻿using Simulator;
using System;
using System.Collections.Generic;

namespace BehaviorTree
{
    public class SimpleEnemyAgent : IAgent
    {
        public (int x, int y) InitialPosition { get; }

        Blackboard bb;

        public SimpleEnemyAgent((int x, int y) InitialPosition) 
        {
            this.InitialPosition = InitialPosition;
            bb = new Blackboard(null, null);
        }

        public IAction PickAction(IState state)
        {
            IEnumerable<IAction> actions = state.GetLegalActionGenerator(this).Generate();
            bb.legalActions = actions;
         
           

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

            bb.previousAction = bb.choosenAction;

            return bb.choosenAction;
        }
    }
}
