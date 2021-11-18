using System;
using System.Collections.Generic;
using System;
using System.Linq;
using Simulator.actioncommands;

namespace BehaviorTree
{
    class MoveNorth : LeafNode
    {
        public MoveNorth(string name, Blackboard blackboard) : base(name, blackboard)
        { }

        public override bool CheckConditions()
        {
            return blackboard.LegalActions != null && blackboard.LegalActions.Any();
        }

        public override void DoAction()
        {
            LogTask("Doing action");
           
            if (blackboard.LegalActions.Any(a => a is GoNorth))
            {
                blackboard.ChoosenAction = blackboard.LegalActions.First(a => a is GoNorth);
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
