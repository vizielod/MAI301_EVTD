using System;
using System.Collections.Generic;

namespace Simulator
{
    enum TileType
    {
        Ground,
        Wall
    }

    class State
    {
        public IEnumerable<Action> Actions { get; }
        public TileType[,] MapLayout { get; }
        public IEnumerable<ActiveAgent> Agents { get; }
        public IEnumerable<Action> LegalActions { get; }

        internal void addAgent(IAgent agent)
        {
            throw new NotImplementedException();
        }

        public float Reward { get; }
    }
}
