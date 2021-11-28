using Simulator.state;
using System.Collections.Generic;
using System.Linq;

namespace Simulator.gamespecific
{
    class TowerDefenceGame : IGame
    {
        private readonly IMapLayout map;
        private readonly Dictionary<IAgent, StateObject> agents;
        private readonly BreadthFirstSearch bfsMap;

        public TowerDefenceGame(IMapLayout map, IEnumerable<IAgent> agents, IEnumerable<IAgent> towers)
        {
            this.map = map;
            this.bfsMap = new BreadthFirstSearch(map);
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

        public IEnumerable<IAgent> ActiveAgents => agents.Where(a => a.Value.IsActive && a.Key.IsActive).Select(a => a.Key);

        public IEnumerable<IAgent> AllAgents => agents.Keys;

        public int CountActiveEnemies()
        {
            return agents.Where(a => a.Key.IsActive && a.Value.IsActive).Count(a => a.Value.IsEnemy);
        }

        public int CountEnemies()
        {
            return agents.Count(a => a.Value.IsEnemy);
        }

        public int CountEnemiesSuccess()
        {
            return agents.Count(a => a.Value.GoalReached && a.Value.IsEnemy);
        }

        public int CountUnspawnedEnemies(int round)
        {
            return agents.Count(a => a.Key.SpawnRound > round && a.Value.IsEnemy);
        }

        public void DespawnAgents(int round)
        {
            agents.Where(a => a.Key.SpawnRound > round).AsParallel().ForAll(a => a.Value.IsActive = false);
        }

        public IState GenerateState()
        {
            var state = new State(map, bfsMap);
            foreach (var agent in agents.Where(a => (a.Value.IsActive && a.Key.IsActive)))
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
