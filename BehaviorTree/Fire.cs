using Simulator.actioncommands;
using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviorTree
{
    class Fire : LeafNode
    {
        public Fire(string name, Blackboard blackboard) : base(name, blackboard)
        { }

        public override bool CheckConditions()
        {
            return blackboard.ClosestEnemy != null && blackboard.IsEnemyInRange; 
        }

        public override void DoAction()
        {
            if (CheckConditions())
            {
                blackboard.ChoosenAction = new QuickAttack(blackboard.Damage, blackboard.ClosestEnemy, blackboard.PreviousTargetEnemy);
                blackboard.PreviousTargetEnemy = blackboard.ClosestEnemy;
                controller.FinishWithSuccess();
            }
            else 
            {
                controller.FinishWithFailure();
            }
        }
    }
}
