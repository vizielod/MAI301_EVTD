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

        public override void End()
        {
            LogTask("Ending");
        }

        public override void LogTask(string log)
        {
            Console.WriteLine("Name: " + name + ", " + log);
        }

        public override void Start()
        {
            LogTask("Starting");
        }
    }
}
