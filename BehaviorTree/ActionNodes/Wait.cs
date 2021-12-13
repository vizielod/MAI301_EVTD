using System;
using BehaviorTree.NodeBase;
using Simulator.actioncommands;

namespace BehaviorTree.ActionNodes
{
    class Wait:LeafNode
    {
        public Wait()
        {
        }

        public override bool CheckConditions()
        {
            return true;
        }

        public override void HandleEnemy(EnemyBlackboard blackboard)
        {

            blackboard.ChoosenAction = new Idle();
            controller.FinishWithSuccess();
            
        }

        public override void HandleTurret(TurretBlackboard blackboard)
        {
            controller.FinishWithFailure();
        }
    }
}
