using BehaviorTree.NodeBase;
using Simulator.actioncommands;

namespace BehaviorTree.Actions
{
    class MoveForward : IActionStrategy
    {
        public bool HandleEnemy(EnemyBlackboard blackboard)
        {
            if (blackboard.ForwardPosition.HasValue && blackboard.CurrentPosition.HasValue)
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

                return true;
            }
            else
                return false;
        }

        public bool HandleTurret(TurretBlackboard blackboard)
        {
            return false;
        }
    }
}
