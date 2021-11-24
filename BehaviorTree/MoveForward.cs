using System;
using Simulator.actioncommands;

namespace BehaviorTree
{
    public class MoveForward: LeafNode
    {
        public MoveForward(string name, Blackboard bb): base(name,bb)
        {
        }

        public override bool CheckConditions()
        {
            return blackboard.ForwardPosition.HasValue && blackboard.CurrentPosition.HasValue;
        }

        public override void DoAction()
        {
            if (CheckConditions())
            {
                int x = blackboard.ForwardPosition.Value.x - blackboard.CurrentPosition.Value.x;
                int y = blackboard.ForwardPosition.Value.y - blackboard.CurrentPosition.Value.y;

                if (x != 0)
                {
                    switch (x)
                    {
                        case 1:
                            blackboard.ChoosenAction = new GoSouth();
                            break;
                        case -1:
                            blackboard.ChoosenAction = new GoNorth();
                            break;
                        default:
                            break;
                    }
                }

                if (y != 0)
                {
                    switch (y)
                    {
                        case 1:
                            blackboard.ChoosenAction = new GoEast();
                            break;
                        case -1:
                            blackboard.ChoosenAction = new GoWest();
                            break;
                        default:
                            break;
                    }
                }

                controller.FinishWithSuccess();
            }
            else
                controller.FinishWithFailure();
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
