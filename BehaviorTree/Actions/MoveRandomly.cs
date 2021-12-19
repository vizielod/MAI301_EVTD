using BehaviorTree.NodeBase;
using System;
using System.Linq;

namespace BehaviorTree.Actions
{
    class MoveRandomly : IActionStrategy
    {
        public ResultEnum Result { get; set; }

        public void HandleEnemy(EnemyBlackboard blackboard)
        {
            if (blackboard.LegalActions != null)
            {
                var rand = new Random();
                blackboard.ChoosenAction = blackboard.LegalActions.ElementAt(rand.Next(blackboard.LegalActions.Count()));
                Result = ResultEnum.Succeeded;
            }
            else
            {
                Result = ResultEnum.Failed;
            }
        }

        public void HandleTurret(TurretBlackboard blackboard)
        {
            Result = ResultEnum.Failed;
        }

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}
