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
                    Type = enemyType
                }) ;
            }
            var towerType = new TowerDefenceTowerAgent();
            foreach (var tower in towers)
            {
                this.agents.Add(tower, new StateObject(tower.InitialPosition)
                {
                    Type = towerType
                });
            }
        }

        public IEnumerable<IAgent> ActiveAgents => agents.Where(a => IsActive(a.Key)).Select(a => a.Key);

        public IEnumerable<IAgent> AllAgents => agents.Keys;

        public IEnumerable<IAgent> AllEnemyAgents => agents.Where(a => a.Value.IsEnemy).Select(a => a.Key);

        public int RoundLimit { get; set; }

        public int CountActiveEnemies()
        {
            return agents.Where(a => IsActive(a.Key)).Count(a => a.Value.IsEnemy);
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
            agents.Where(a => a.Key.SpawnRound > round).AsParallel().ForAll(a => a.Value.Spawned = false);
        }

        public void Disable(IAgent agent)
        {
            agents[agent].IsEnabled = false;
        }

        public void VerifyPositions()
        {
            foreach (var agent in agents.Where(a => a.Key.IsEnemy && IsActive(a.Key)))
            {
                if (!bfsMap.HasNext(agent.Value.GridLocation))
                    Disable(agent.Key);
                (int x, int y) = agent.Value.GridLocation;
                if (map.InBounds(x, y) && map.TypeAt(x, y) == TileType.Goal)
                {
                    agent.Value.GoalReached = true;
                    agent.Value.IsEnabled = false;
                }
            }
        }

        public IState GenerateState()
        {
            var state = new State(map, bfsMap);
            foreach (var agent in agents.Where(a => IsActive(a.Key)))
            {
                state.AddAgent(agent.Key, agent.Value.GridLocation, agent.Value.Type);
                state.SetTarget(agent.Key, agent.Value.Target, agent.Value.EngagedTarget);
            }

            return state;
        }

        public float GetProgression(IAgent agent)
        {
            // Invert distance to goal to show progression towards the goal.
            return 1 - bfsMap.Distance(agents[agent].GridLocation);
        }

        public IStateObject GetStateObject(IAgent agent)
        {
            return agents[agent];
        }

        public bool IsActive(IAgent agent)
        {
            return agent.IsActive && agents[agent].Spawned && agents[agent].IsEnabled;
        }

        public void NewRound()
        {
            // Reset temporary states
            agents.AsParallel().ForAll(a =>
            {
                a.Value.EngagedTarget = false;
            });
        }

        public void SpawnAgents(int round)
        {
            agents.Where(a => a.Key.SpawnRound <= round).AsParallel().ForAll(a => a.Value.Spawned = true);
        }
    }
}
