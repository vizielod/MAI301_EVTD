using BehaviorTree.Conditionals;
using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviorTree.NodeBase
{
    class ConditionalNode : LeafNode
    {
        public ConditionalNode(IConditionStrategy strategy)
        {
            Strategy = strategy;
        }

        public IConditionStrategy Strategy { get; set; }

        public override bool CheckConditions()
        {
            return true;
        }

        public override Node DeepCopy()
        {
            return new ConditionalNode(Strategy);
        }

        public override void HandleEnemy(EnemyBlackboard blackboard)
        {
            if (Strategy.HandleEnemy(blackboard))
                controller.FinishWithSuccess();
            else
                controller.FinishWithFailure();
        }

        public override void HandleTurret(TurretBlackboard blackboard)
        {
            if (Strategy.HandleTurret(blackboard))
                controller.FinishWithSuccess();
            else
                controller.FinishWithFailure();
        }
    }
}
