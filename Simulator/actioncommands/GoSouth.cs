using Simulator.state;

namespace Simulator.actioncommands
{
    class GoSouth : IAction
    {
        private readonly StateObject obj;

        public GoSouth(StateObject obj)
        {
            this.obj = obj;
        }

        public void Apply()
        {
            obj.GridLocation = (obj.GridLocation.x, obj.GridLocation.y - 1);
        }

        public void Undo()
        {
            obj.GridLocation = (obj.GridLocation.x, obj.GridLocation.y + 1);
        }
    }
}
