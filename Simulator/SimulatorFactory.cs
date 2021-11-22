using Simulator.gamespecific;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Simulator
{
    public class SimulatorFactory
    {
        public IStateSequence CreateSimulator(IMapLayout initialMap, IEnumerable<IAgent> agents, IEnumerable<IAgent> towers)
        {
            if (initialMap.Count(t => t == TileType.Goal) != 1)
                throw new ArgumentException("Initial map must have exactly one goal", nameof(initialMap));

            var game = new TowerDefenceGame(initialMap, agents, towers);
            return new Simulator(game);
        }
    }
}
