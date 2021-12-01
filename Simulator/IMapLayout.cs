using System.Collections.Generic;

namespace Simulator
{
    public enum TileType
    {
        Ground,
        Wall,
        Spawn,
        Goal,
        Turret
    }

    public interface IMapLayout : IEnumerable<TileType>
    {
        TileType TypeAt(int x, int y);
        int Height { get; }
        int Width { get; }
        (int x, int y) Spawn { get; }
        (int x, int y) Goal { get; }
        (int x, int y) Translate(Direction direction);
        Direction Translate(int x, int y);
        bool InBounds(int x, int y);
    }
}
