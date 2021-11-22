using Simulator.actioncommands;
using System.Collections.Generic;

namespace Simulator.state
{
    class LegalMoveGenerator : IActionGenerator
    {
        private readonly IMapLayout map;
        private readonly StateObject agent;
        private List<TileType> groundTiles = new List<TileType> { TileType.Ground, TileType.Goal, TileType.Spawn };

        public LegalMoveGenerator(IMapLayout map, StateObject agent)
        {
            this.map = map;
            this.agent = agent;
        }

        public IEnumerable<IAction> Generate()
        {
            (int x, int y) = agent.GridLocation;
            if (IsGround(map.TypeAt(x - 1, y)))
                yield return new GoNorth();
            if (IsGround(map.TypeAt(x + 1, y)))
                yield return new GoSouth();
            if (IsGround(map.TypeAt(x, y + 1)))
                yield return new GoEast();
            if (IsGround(map.TypeAt(x, y - 1)))
                yield return new GoWest();

            if (map.TypeAt(x, y) == TileType.Goal)
                yield return new ScorePoints();
        }

        private bool IsGround(TileType type) => groundTiles.Contains(type);
    }
}
