using Simulator.state;

namespace Simulator.actioncommands
{
    class GoNorth : IAction
    {
        private readonly StateObject obj;

        public GoNorth(StateObject obj)
        {
            this.obj = obj;
        }

        public void Apply()
        {
            obj.GridLocation = (obj.GridLocation.x, obj.GridLocation.y + 1);
        }

        public Direction GetDirection()
        {
            return Direction.North;
        }

        public void Undo()
        {
            obj.GridLocation = (obj.GridLocation.x, obj.GridLocation.y - 1);
        }
    }
}
