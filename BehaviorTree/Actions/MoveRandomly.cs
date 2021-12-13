using BehaviorTree.NodeBase;
using System;
using System.Linq;

namespace BehaviorTree.Actions
{
    class MoveRandomly : IActionStrategy
    {
        public bool HandleEnemy(EnemyBlackboard blackboard)
        {
            if (blackboard.LegalActions != null)
            {
                var rand = new Random();
                blackboard.ChoosenAction = blackboard.LegalActions.ElementAt(rand.Next(blackboard.LegalActions.Count()));
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool HandleTurret(TurretBlackboard blackboard)
        {
            return false;
        }
    }
}
