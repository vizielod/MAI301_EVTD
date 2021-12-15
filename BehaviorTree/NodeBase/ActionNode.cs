using System.Collections.Generic;
using BehaviorTree.Actions;

namespace BehaviorTree.NodeBase
{
    class ActionNode : LeafNode
    {
        public ActionNode(IActionStrategy strategy)
        {
            Strategy = strategy;
        }

        public IActionStrategy Strategy { get; set; }

        public override bool CheckConditions()
        {
            return true;
        }

        public override Node DeepCopy()
        {
            return new ActionNode(Strategy);
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
