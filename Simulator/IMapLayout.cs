using System.Collections.Generic;

namespace Simulator
{
    public enum TileType
    {
        Ground,
        Wall,
        Spawn,
        Goal
    }

    public interface IMapLayout : IEnumerable<TileType>
    {
        TileType TypeAt(int x, int y);
        int Height { get; }
        int Width { get; }
        (int x, int y) GetSpawnPoint();
    }
}
