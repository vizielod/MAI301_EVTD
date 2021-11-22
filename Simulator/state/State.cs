using Simulator.gamespecific;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Simulator.state
{
    class State : IState
    {
        public IMapLayout MapLayout { get; }
        public IEnumerable<IAgent> Agents => agents.Select(p => p.Key);

        private IDictionary<IAgent, StateObject> agents;
        private List<Event> events;
        private readonly BroadFirstSearch bfs;

        internal IEnumerable<Event> Events => events;

        public State(IMapLayout map, BroadFirstSearch bfs)
        {
            MapLayout = map;
            this.bfs = bfs;
            agents = new Dictionary<IAgent, StateObject>();
        }

        public void AddAgent(IAgent agent, (int x, int y) gridLocation, IAgentType type)
        {
            agents.Add(agent, new StateObject(gridLocation) { 
                IsActive = true, 
                Type = type
            });
        }

        public IEnumerable<IAgent> GetAgentsAt(int x, int y)
        {
            return agents.Where(p => p.Value.GridLocation == (x, y)).Select(p => p.Key);            
        }

        public (int x, int y) PositionOf(IAgent agent)
        {
            return agents[agent].GridLocation;
        }

        public IActionGenerator GetLegalActionGenerator(IAgent agent)
        {
            return agents[agent].GetLegalActionGenerator(MapLayout);
        }

        public Maybe<IAgent> GetClosestEnemy(IAgent agent)
        {
            if (!agents.ContainsKey(agent))
                return new Maybe<IAgent>();

            double closestSQDistance = double.MaxValue;
            IAgent closest = null;

            foreach (var enemy in agents.Where(a => (a.Key.IsActive && a.Value.Type.IsEnemy && a.Key != agent)))
            {
                var squaredDistance = Mathf.Pow(enemy.Value.GridLocation.x - agents[agent].GridLocation.x, 2) + Mathf.Pow(enemy.Value.GridLocation.y - agents[agent].GridLocation.y, 2);

                if (squaredDistance < closestSQDistance)
                {
                    closest = enemy.Key;
                    closestSQDistance = squaredDistance;
                }
            }

            return Maybe.Create(closest);
        }

        public Maybe<IAgent> GetTargetOf(IAgent agent)
        {
            return Maybe.Create(agents[agent].Target);
        }

        public (int x, int y) SuggestPosition(IAgent agent)
        {
            return bfs.Next(agents[agent].GridLocation);
        }
    }
}
