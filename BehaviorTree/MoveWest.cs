using System;
using System.Linq;
using Simulator.actioncommands;

namespace BehaviorTree
{
    class MoveWest : LeafNode
    {
        public MoveWest(string name, Blackboard blackboard) : base(name, blackboard)
        { }

        public override bool CheckConditions()
        {
            return blackboard.legalActions != null && blackboard.legalActions.Any();
        }

        public override void DoAction()
        {
            LogTask("Doing action");
            if (blackboard.legalActions.OfType<GoWest>().Any())
            {
                blackboard.choosenAction = blackboard.legalActions.OfType<GoWest>().First();
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
