﻿using Simulator.state;
using System.Collections.Generic;
using System.Linq;

namespace Simulator.gamespecific
{
    class TowerDefenceGame : IGame
    {
        private readonly IMapLayout map;
        private readonly Dictionary<IAgent, StateObject> agents;

        public TowerDefenceGame(IMapLayout map, IEnumerable<IAgent> agents, IEnumerable<IAgent> towers)
        {
            this.map = map;
            this.agents = new Dictionary<IAgent, StateObject>();
            var enemyType = new TowerDefenceEnemyAgent();
            foreach (var agent in agents)
            {
                this.agents.Add(agent, new StateObject(agent.InitialPosition)
                {
                    IsActive = false,
                    Type = enemyType
                }) ;
            }
            var towerType = new TowerDefenceTowerAgent();
            foreach (var tower in towers)
            {
                this.agents.Add(tower, new StateObject(tower.InitialPosition)
                {
                    IsActive = false,
                    Type = towerType
                });
            }
        }

        public IEnumerable<IAgent> Agents => agents.Where(a => a.Value.IsActive && a.Key.IsActive).Select(a => a.Key);

        public void DespawnAgents(int round)
        {
            agents.Where(a => a.Key.SpawnRound > round).AsParallel().ForAll(a => a.Value.IsActive = false);
        }

        public IState GenerateState()
        {
            var state = new State(map);
            foreach (var agent in agents)
                state.AddAgent(agent.Key, agent.Value.GridLocation, agent.Value.Type);

            return state;
        }

        public IStateObject GetStateObject(IAgent agent)
        {
            return agents[agent];
        }

        public void SpawnAgents(int round)
        {
            agents.Where(a => a.Key.SpawnRound <= round).AsParallel().ForAll(a => a.Value.IsActive = true);
        }
    }
}