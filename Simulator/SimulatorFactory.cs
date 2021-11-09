using Simulator.state;
using System;
using System.Linq;

namespace Simulator
{
    public class SimulatorFactory
    {
        public IStateSequence CreateSimulator(IMapLayout initialMap)
        {
            if (initialMap.Count(t => t == TileType.Goal) != 1)
                throw new ArgumentException("Initial map must have exactly one goal", nameof(initialMap));

            var state = new State(initialMap);
            (int x, int y) = initialMap.GetSpawnPoints().First();
            state.AddAgent(new DummyAgent(), x, y);
            return new Simulator(state);
        }

        class DummyAgent : IAgent
        {
            public IAction PickAction(IState state)
            {
                return state.GetLegalActionGenerator(this).Generate().First();
            }
        }
    }
}
