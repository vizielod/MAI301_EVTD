using Simulator.gamespecific;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Simulator
{
    public class TowerDefenceConfiguration
    {
        public int PlayLifes { get; set; }
        public int MaxRounds { get; set; }
        public IMapLayout Map { get; set; }

    }

    public class TowerDefenceSimulatorFactory
    {
        private readonly TowerDefenceConfiguration config;
        private readonly IEnumerable<IAgent> towers;

        public TowerDefenceSimulatorFactory(TowerDefenceConfiguration config, IEnumerable<IAgent> towers)
        {
            this.config = config;
            this.towers = towers;
        }

        public IStateSequence Create(IEnumerable<IAgent> agents)
        {
            var game = new TowerDefenceGame(config.Map, agents, towers)
            {
                RoundLimit = config.MaxRounds,
                PlayerLifes = config.PlayLifes
            };
            return new Simulator(game);
        }
    }
}
