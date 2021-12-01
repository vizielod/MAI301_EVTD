using System;
using System.Collections.Generic;
using System.Text;

namespace Simulator.actioncommands
{
    public class Idle : IAction
    {
        public void Apply(IStateObject stateObject)
        { }
        public void Undo(IStateObject stateObject)
        { }
    }
}
