﻿using Simulator.state;

namespace Simulator.actioncommands
{
    public class GoEast : IAction
    {
        private readonly StateObject obj;

        public GoEast(StateObject obj)
        {
            this.obj = obj;
        }

        public void Apply()
        {
            obj.GridLocation = (obj.GridLocation.x + 1, obj.GridLocation.y);
        }

        public void Undo()
        {
            obj.GridLocation = (obj.GridLocation.x + 1, obj.GridLocation.y);
        }
    }
}
