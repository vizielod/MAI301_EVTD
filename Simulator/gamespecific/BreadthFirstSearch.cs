using System;
using System.Collections.Generic;
using System.Linq;

namespace Simulator.gamespecific
{
    class BreadthFirstSearch
    {
        private readonly Dictionary<(int x, int y), (int x, int y)> lookup;
        private readonly Dictionary<(int x, int y), int> goalDistance;
        private int maxDistance = 0;
        private readonly List<TileType> groundTiles = new List<TileType> { TileType.Spawn, TileType.Ground, TileType.Goal };
        private readonly IMapLayout map;

        public BreadthFirstSearch(IMapLayout map)
        {
            this.map = map;
            lookup = new Dictionary<(int x, int y), (int x, int y)>();
            goalDistance = new Dictionary<(int x, int y), int>();
            BakeMap();
        }

        public float Distance((int x, int y) pos)
        {
            return goalDistance[pos] / maxDistance;
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
            {
                frontér.Enqueue(neighbour);
                lookup.Add(neighbour, map.Goal);
            }
            
            while (frontér.Count > 0)
            {
                var current = frontér.Dequeue();
                
                foreach (var neighbour in GetNeighbours(current).Where(n => !(frontér.Contains(n) || lookup.Keys.Contains(n))))
                {
                    frontér.Enqueue(neighbour);
                    lookup.Add(neighbour, current);
                }
            }

            foreach (var pos in lookup.Keys)
            {
                var distance = CountLookups(pos);
                goalDistance.Add(pos, distance);
                if (distance > maxDistance)
                    maxDistance = distance;
            }
        }

        private int CountLookups((int x, int y) pos)
        {
            int counter = 1;
            while ((pos = lookup[pos]) != (-1, -1))
                counter++;
            return counter;
        }

        private IEnumerable<(int x, int y)> GetNeighbours((int x, int y) value)
        {
            (int x, int y) = value;
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                var dir = map.Translate(direction);
                var x_pos = x + dir.x;
                var y_pos = y + dir.y;
                
                if (map.InBounds(x_pos, y_pos) && IsGround(map.TypeAt(x_pos, y_pos)))
                {
                    yield return (x_pos, y_pos);
                }
            }
        }

        private bool IsGround(TileType type) => groundTiles.Contains(type);
    }
}
