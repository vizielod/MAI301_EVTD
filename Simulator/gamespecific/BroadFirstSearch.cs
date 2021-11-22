using System.Collections.Generic;
using System.Linq;

namespace Simulator.gamespecific
{
    class BroadFirstSearch
    {
        private readonly Dictionary<(int x, int y), (int x, int y)> lookup;
        private readonly List<TileType> groundTiles = new List<TileType> { TileType.Spawn, TileType.Ground, TileType.Goal };
        private readonly IMapLayout map;

        public BroadFirstSearch(IMapLayout map)
        {
            this.map = map;
            lookup = new Dictionary<(int x, int y), (int x, int y)>();
            BakeMap();
        }

        public (int x, int y) Next((int x, int y) pos)
        {
            return lookup[pos];
        }

        private void BakeMap()
        {
            lookup.Add(map.Goal, (-1, -1));
            Queue<(int x, int y)> frontér = new Queue<(int x, int y)>();
            foreach (var neighbour in GetNeighbours(map.Goal))
                frontér.Enqueue(neighbour);

            (int x, int y) previous = map.Goal;
            while (frontér.Count > 0)
            {
                var current = frontér.Dequeue();
                lookup.Add(current, previous);
                foreach (var neighbour in GetNeighbours(map.Goal).Where(n => !(frontér.Contains(n) || lookup.Keys.Contains(n))))
                    frontér.Enqueue(neighbour);

                previous = current;
            }
        }

        private IEnumerable<(int x, int y)> GetNeighbours((int x, int y) value)
        {
            (int x, int y) = value;
            if (IsGround(map.TypeAt(x - 1, y)))
                yield return (x - 1, y);
            if (IsGround(map.TypeAt(x + 1, y)))
                yield return (x + 1, y);
            if (IsGround(map.TypeAt(x, y + 1)))
                yield return (x, y + 1);
            if (IsGround(map.TypeAt(x, y - 1)))
                yield return (x, y - 1);
        }

        private bool IsGround(TileType type) => groundTiles.Contains(type);
    }
}
