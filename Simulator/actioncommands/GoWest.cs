using Simulator.state;

namespace Simulator.actioncommands
{
    public class GoWest : IAction
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

        public void Undo()
        {
            obj.GridLocation = (obj.GridLocation.x - 1, obj.GridLocation.y);
        }
    }
}
