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
            return !blackboard.ForwardPosition.Equals(default(ValueTuple<int, int, int>)) && !blackboard.CurrentPosition.Equals(default(ValueTuple<int, int, int>));
        }

        public override void DoAction()
        {
            if (CheckConditions())
            {
                int x = blackboard.ForwardPosition.x - blackboard.CurrentPosition.x;
                int y = blackboard.ForwardPosition.y - blackboard.CurrentPosition.y;

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
