using BehaviorTree.ActionNodes;
using BehaviorTree.FlowControllNodes;
using BehaviorTree.NodeBase;
using Simulator;
using Simulator.actioncommands;
using System;

namespace BehaviorTree
{
    public class TurretAgent : IAgent
    {
        TurretBlackboard bb;
        int damage;

        public IAgent Target { get; set; } = null;
        public (int x, int y) InitialPosition { get; }

        public int SpawnRound => 0;

        public bool IsActive => true;

        public float Range { get; set; }

        public bool IsEnemy => false;

        public float HealthRatio => 1;

        public TurretAgent((int x, int y) InitialPosition)
        {
            this.InitialPosition = InitialPosition;
            bb = new TurretBlackboard();
            Range = 1.0f;
            damage = 2;
        }

        public IAction PickAction(IState state)
        {
            Target = null;
            bb.ChoosenAction = null;
            state.GetClosestEnemy(this).Apply(closest =>
            {
                bb.ClosestEnemy = closest;
                bb.Damage = damage;
                bb.IsEnemyInRange = false;
                // Calculate the distance
                (int x, int y) turretPos = state.PositionOf(this);
                (int x, int y) enemyPos = state.PositionOf(closest);
                (int x, int y) p = (enemyPos.x - turretPos.x, enemyPos.y - turretPos.y);
                if (Math.Max(Math.Abs(p.x), Math.Abs(p.y)) <= Range)
                {
                    bb.IsEnemyInRange = true;
                    Target = closest;
                }

                Selector move = new Selector();
                move.AddChildren(new Fire());

                move.Start();

                while (move.Running())
                {
                    move.DoAction(bb);
                }

                move.End();

                bb.PreviousAction = bb.ChoosenAction;
            });
            if (bb.ChoosenAction == null)
            {
                return new Idle();
            }
            return bb.ChoosenAction;
        }

        public void Damage(int v)
        {
            throw new NotImplementedException();
        }

        public void Heal(int v)
        {
            throw new NotImplementedException();
        }
    }
}