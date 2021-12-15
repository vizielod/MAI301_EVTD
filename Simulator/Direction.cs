using System;

namespace Simulator
{
    [Flags]public enum Direction
    {
        North = 1 << 0,
        South = 1 << 1,
        East = 1 << 2,
        West = 1 << 3
    }
}