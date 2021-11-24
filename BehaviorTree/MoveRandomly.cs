using System;
using System.Linq;
using Simulator.actioncommands;

namespace BehaviorTree
{
    public class MoveRandomly:LeafNode
    {
        public MoveRandomly(string name, Blackboard bb):base(name, bb)
        {
        }

        public override bool CheckConditions()
        {
            return blackboard.LegalActions != null;
        }

        public override void DoAction()
        {
            LogTask("Doing action");

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
