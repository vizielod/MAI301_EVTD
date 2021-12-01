using Simulator.actioncommands;
using System;
using System.Collections.Generic;

namespace Simulator.gamespecific
{
    class RigidMoveGenerator : IActionGenerator
    {
        private readonly IMapLayout map;
        private readonly IStateObject agentState;
        private List<TileType> groundTiles = new List<TileType> { TileType.Ground, TileType.Goal, TileType.Spawn };
        private Dictionary<Direction, IAction> translations = new Dictionary<Direction, IAction> {
            { Direction.North, new GoNorth() },
            { Direction.South, new GoSouth() },
            { Direction.East, new GoEast() },
            { Direction.West, new GoWest() },
        };

        public RigidMoveGenerator(IMapLayout map, IStateObject agentState)
        {
            this.map = map;
            this.agentState = agentState;
        }

        public IEnumerable<IAction> Generate()
        {
            (int x, int y) = agentState.GridLocation;
            foreach(Direction direction in Enum.GetValues(typeof(Direction)))
            {
                (int i, int j) = map.Translate(direction);
                if (IsGround(map.TypeAt(x + i, y + j)))
                    yield return translations[direction];
            }
        }

        private bool IsGround(TileType type) => groundTiles.Contains(type);
    }
}
