using System.Collections.Generic;

namespace Simulator
{
    public enum TileType
    {
        Ground,
        Wall
    }

    public interface IState
    {
        TileType[,] MapLayout { get; }
        IEnumerable<IAgent> Agents { get; }
        IEnumerable<IAgent> GetAgentsAt(int x, int y);
        (int x, int y) PositionOf(IAgent agent);
    }
}