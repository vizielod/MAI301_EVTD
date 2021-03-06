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
        int ScoredPoints { get; }
        IEnumerable<IAgent> Agents { get; }
        IEnumerable<IAgent> GetAgentsAt(int x, int y);
        (int x, int y) PositionOf(IAgent agent);
        IActionGenerator GetLegalActionGenerator(IAgent agent);
        Maybe<IAgent> GetClosestEnemy(IAgent agent);
        Maybe<IAgent> GetClosestTurret(IAgent agent);
        Maybe<IAgent> GetTargetOf(IAgent agent);
        bool EngagedTargetOf(IAgent agent);
        Maybe<(int x, int y)> SuggestPosition(IAgent agent);
        Alliances? Winner { get; set; }
        IEnumerable<IAgent> GetTurretsAttacking(IAgent agent);
        IAction SuggestedAction(IAgent agent);
        Direction GetDirection(IAgent from, IAgent to);
        IReadOnlyDictionary<Direction, int> GetWallDistances(IAgent agent);
        bool IsActive(IAgent agent);
    }
}