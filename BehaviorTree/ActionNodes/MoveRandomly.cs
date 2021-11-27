using BehaviorTree.NodeBase;
using System;
using System.Linq;

namespace BehaviorTree.ActionNodes
{
    class MoveRandomly:LeafNode
    {
        private readonly EnemyBlackboard blackboard;

        public MoveRandomly( EnemyBlackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        public override bool CheckConditions()
        {
            return blackboard.LegalActions != null;
        }

        public override void DoAction()
        {

            if (CheckConditions())
            {
                var rand = new Random();
                blackboard.ChoosenAction = blackboard.LegalActions.ElementAt(rand.Next(blackboard.LegalActions.Count()));
                controller.FinishWithSuccess();
            }
            else
            {
                controller.FinishWithFailure();
            }
        }
    }
}
