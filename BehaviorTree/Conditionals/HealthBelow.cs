using System;
using BehaviorTree.NodeBase;

namespace BehaviorTree.Conditionals
{
    class HealthBelow:IConditionStrategy
    {
        private readonly float healthRatio;

        public HealthBelow(float healthRatio)
        {
            this.healthRatio = healthRatio.Clamp(0,1);
        }

        public bool HandleEnemy(EnemyBlackboard blackboard)
        {
            if (blackboard.HealthRatio < healthRatio)
                return true;
            else
                return false;
        }

        public bool HandleTurret(TurretBlackboard blackboard)
        {
            return false;
        }

    }
}
