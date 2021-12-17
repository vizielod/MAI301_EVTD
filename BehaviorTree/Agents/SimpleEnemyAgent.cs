﻿using BehaviorTree.Actions;
using BehaviorTree.Agents;
using BehaviorTree.Conditionals;
using BehaviorTree.FlowControllNodes;
using BehaviorTree.NodeBase;
using Simulator;
using System;
using System.Collections.Generic;

namespace BehaviorTree
{
    public class SimpleEnemyAgent : IEnemyAgent
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

            bb.ProgressiveAction = state.SuggestedAction(this);
            bb.CurrentPosition = state.PositionOf(this);
            bb.WallDistances = state.GetWallDistances(this);

            Selector move = new Selector();

            move.AddChildren(new ConditionalNode(new CanMoveSouth()));
            move.AddChildren(new ActionNode(new ContinuousMovement(new MoveSouth(), Direction.South)));
            move.AddChildren(new ActionNode(new ContinuousMovement(new MoveEast(), Direction.East)));

            move.Start();

            while (move.Running()) 
            {
                move.DoAction(bb);
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
    }
}
