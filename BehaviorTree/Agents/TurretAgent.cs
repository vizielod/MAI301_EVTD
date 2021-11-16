using Simulator;
using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviorTree
{
    class TurretAgent : IAgent
    {
        Blackboard bb;
        float radius;
        float damage;

        public (int x, int y) InitialPosition { get; }

        public TurretAgent((int x, int y) InitialPosition)
        {
            this.InitialPosition = InitialPosition;
            bb = new Blackboard(null, null);
            radius = 1.0f;
            damage = 2.0f;
        }

        public IAction PickAction(IState state)
        {
            IAgent closest = state.GetClosestAgent(this);

            bb.closestEnemy = closest;
            // Calculate the distance
            (int x, int y) turretPos = state.PositionOf(this);
            (int x, int y) enemyPos = state.PositionOf(closest);

            if(enemyPos.x-turretPos.x)

            throw new NotImplementedException();
        }
    }
}
