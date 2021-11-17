using Simulator;
using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviorTree
{
    public class TurretAgent : IAgent
    {
        Blackboard bb;
        float range;
        int damage;

        public (int x, int y) InitialPosition { get; }

        public int spawnRound => throw new NotImplementedException();

        public TurretAgent((int x, int y) InitialPosition)
        {
            this.InitialPosition = InitialPosition;
            bb = new Blackboard(null, null);
            range = 1.0f;
            damage = 2;
        }

        public IAction PickAction(IState state)
        {
            state.GetClosestEnemy(this).Apply(closest =>
            {
                bb.ClosestEnemy = closest;
                bb.Damage = damage;
                bb.IsEnemyInRange = false;
                // Calculate the distance
                (int x, int y) turretPos = state.PositionOf(this);
                (int x, int y) enemyPos = state.PositionOf(closest);
                (int x, int y) p = (enemyPos.x - turretPos.x, enemyPos.y - turretPos.y);
                if (Math.Max(Math.Abs(p.x), Math.Abs(p.y)) <= range)
                    bb.IsEnemyInRange = true;

                Selector move = new Selector("Selector", bb);
                ((ParentNodeController)move.GetControl()).
                    AddNode(new Fire(
                    "Fire", bb));
                ((ParentNodeController)move.GetControl()).
                    AddNode(new Rotate(
                    "Rotate", bb));

                ((ParentNodeController)move.GetControl()).SafeStart();

                while (!((ParentNodeController)move.GetControl()).Finished())
                {
                    move.DoAction();
                }

            ((ParentNodeController)move.GetControl()).SafeEnd();

                bb.PreviousAction = bb.ChoosenAction;
            });
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
