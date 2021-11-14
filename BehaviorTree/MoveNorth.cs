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
            return blackboard.legalActions != null && blackboard.legalActions.Any();
        }

        public override void DoAction()
        {
            if (blackboard.legalActions.Any(a => a.GetDirection() == blackboard.previousAction.GetDirection()))
            {
                if (blackboard.previousAction.GetDirection() == Simulator.Direction.North)
                {
                    blackboard.choosenAction = blackboard.legalActions.First(a => a.GetDirection() == Simulator.Direction.East);
                    controller.FinishWithSuccess();
                }
                else
                {
                    controller.FinishWithFailure();
                }
            }
            else
            {
                if (blackboard.legalActions.Any(a => a.GetDirection() == Simulator.Direction.North))
                {
                    blackboard.choosenAction = blackboard.legalActions.First(a => a.GetDirection() == Simulator.Direction.North);
                    controller.FinishWithSuccess();
                }
                else
                {
                    controller.FinishWithFailure();
                }
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
