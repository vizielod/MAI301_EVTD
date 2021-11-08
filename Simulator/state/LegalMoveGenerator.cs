using Simulator.actioncommands;
using System.Collections.Generic;

namespace Simulator.state
{
    class LegalMoveGenerator : IActionGenerator
    {
        private readonly IMapLayout map;
        private readonly StateObject agent;

        public LegalMoveGenerator(IMapLayout map, StateObject agent)
        {
            this.map = map;
            this.agent = agent;
        }

        public IEnumerable<IAction> Generate()
        {
            (int x, int y) = agent.GridLocation;
            if (map.TypeAt(x, y + 1) == TileType.Ground)
                yield return new GoNorth(agent);
            if (map.TypeAt(x, y - 1) == TileType.Ground)
                yield return new GoSouth(agent);
            if (map.TypeAt(x + 1, y) == TileType.Ground)
                yield return new GoEast(agent);
            if (map.TypeAt(x - 1, y) == TileType.Ground)
                yield return new GoWest(agent);
        }
    }
}
