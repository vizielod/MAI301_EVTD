using System.Collections.Generic;

namespace Simulator.actioncommands
{
    class NoActionGenerator : IActionGenerator
    {
        public IEnumerable<IAction> Generate() => new IAction[0];
    }
}
