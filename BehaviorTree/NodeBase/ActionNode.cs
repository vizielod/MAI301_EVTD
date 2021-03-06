using System.Collections.Generic;
using BehaviorTree.Actions;

namespace BehaviorTree.NodeBase
{
    class ActionNode : LeafNode
    {
        public override NodeType Type => NodeType.Action;
        public override string ToString()
        {
            return Strategy.ToString();
        }

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
            Strategy.HandleEnemy(blackboard);
            if (Strategy.Result == ResultEnum.Succeeded)
                controller.FinishWithSuccess();
            else if (Strategy.Result == ResultEnum.Failed)
                controller.FinishWithFailure();
            else
                controller.FinishWithRunning();
        }

        public override void HandleTurret(TurretBlackboard blackboard)
        {
            Strategy.HandleTurret(blackboard);
            if (Strategy.Result == ResultEnum.Succeeded)
                controller.FinishWithSuccess();
            else if (Strategy.Result == ResultEnum.Failed)
                controller.FinishWithFailure();
            else
                controller.FinishWithRunning();
        }
    }
}
