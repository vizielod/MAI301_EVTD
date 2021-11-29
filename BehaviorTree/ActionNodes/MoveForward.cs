using BehaviorTree.NodeBase;
using Simulator.actioncommands;

namespace BehaviorTree.ActionNodes
{
    class MoveForward: LeafNode
    {
         EnemyBlackboard blackboard;

        public MoveForward(EnemyBlackboard blackboard)
        {
            this.blackboard = blackboard;
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
    }
}
