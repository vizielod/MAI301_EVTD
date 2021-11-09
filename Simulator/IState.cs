using System.Collections.Generic;

namespace Simulator
{
    public interface IState
    {
        IEnumerable<IAgent> Agents { get; }
        IEnumerable<IAgent> GetAgentsAt(int x, int y);
        (int x, int y) PositionOf(IAgent agent);
        IActionGenerator GetLegalActionGenerator(IAgent agent);
    }
}