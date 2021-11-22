using Simulator.actioncommands;
using System.Collections.Generic;

namespace Simulator.gamespecific
{
    class RigidMoveGenerator : IActionGenerator
    {
        private readonly IMapLayout map;
        private readonly IStateObject agentState;
        private List<TileType> groundTiles = new List<TileType> { TileType.Ground, TileType.Goal, TileType.Spawn };

        public RigidMoveGenerator(IMapLayout map, IStateObject agentState)
        {
            this.map = map;
            this.agentState = agentState;
        }

        public IEnumerable<IAction> Generate()
        {
            (int x, int y) = agentState.GridLocation;
            if (IsGround(map.TypeAt(x - 1, y)))
                yield return new GoNorth();
            if (IsGround(map.TypeAt(x + 1, y)))
                yield return new GoSouth();
            if (IsGround(map.TypeAt(x, y + 1)))
                yield return new GoEast();
            if (IsGround(map.TypeAt(x, y - 1)))
                yield return new GoWest();
        }

        private bool IsGround(TileType type) => groundTiles.Contains(type);
    }
}
