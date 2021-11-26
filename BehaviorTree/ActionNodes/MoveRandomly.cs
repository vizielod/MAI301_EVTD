using BehaviorTree.NodeBase;
using System;
using System.Linq;

namespace BehaviorTree.ActionNodes
{
    public class MoveRandomly:LeafNode
    {
        public MoveRandomly( Blackboard bb):base( bb)
        {
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
