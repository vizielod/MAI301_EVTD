using Simulator.state;

namespace Simulator.actioncommands
{
    class GoWest : IAction
    {
        private readonly StateObject obj;

        public GoWest(StateObject obj)
        {
            this.obj = obj;
        }

        public void Apply()
        {
            obj.GridLocation = (obj.GridLocation.x - 1, obj.GridLocation.y);
        }

        public Direction GetDirection()
        {
            return Direction.West;
        }

        public void Undo()
        {
            obj.GridLocation = (obj.GridLocation.x + 1, obj.GridLocation.y);
        }
    }
}
