using System.Collections.Generic;

namespace Simulator
{
    public enum Alliances
    {
        Player,
        Enemies
    }

    public interface IState
    {
        IEnumerable<IAgent> Agents { get; }
        IEnumerable<IAgent> GetAgentsAt(int x, int y);
        (int x, int y) PositionOf(IAgent agent);
        IActionGenerator GetLegalActionGenerator(IAgent agent);
        Maybe<IAgent> GetClosestEnemy(IAgent agent);
        Maybe<IAgent> GetTargetOf(IAgent agent);
        (int x, int y) SuggestPosition(IAgent agent);
        Alliances? Winner { get; set; }
    }
}