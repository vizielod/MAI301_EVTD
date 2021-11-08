using System.Collections.Generic;

namespace Simulator
{
    public interface IActionGenerator
    {
        IEnumerable<IAction> Generate();
    }
}
