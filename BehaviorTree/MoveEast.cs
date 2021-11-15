using System;
using System.Linq;
using Simulator.actioncommands;

namespace BehaviorTree
{
    class MoveEast : LeafNode
    {
        public MoveEast(string name, Blackboard blackboard):base(name, blackboard) 
        { }

        public override bool CheckConditions()
        {
            return blackboard.legalActions != null && blackboard.legalActions.Any();
        }

        public override void DoAction()
        {
            LogTask("Doing action");
            if (blackboard.legalActions.Any(a => a is GoEast)) 
            {
                blackboard.choosenAction = blackboard.legalActions.First(a => a is GoEast);
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
