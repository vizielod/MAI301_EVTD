using System;
using System.Collections.Generic;
using System.Linq;

namespace Simulator.state
{
    class State : IState
    {
        public IMapLayout MapLayout { get; }
        public IEnumerable<IAgent> Agents => agents.Select(p => p.Key);

        private IDictionary<IAgent, StateObject> agents;
        private List<Event> events;
        internal IEnumerable<Event> Events => events;

        public State(IMapLayout map)
        {
            MapLayout = map;
            agents = new Dictionary<IAgent, StateObject>();
        }

        public void AddAgent(IAgent agent, (int x, int y) gridLocation, AgentType type)
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
            return new LegalMoveGenerator(MapLayout, agents[agent]); // TODO: Fix Control Freak anti pattern
        }

        public Maybe<IAgent> GetClosestEnemy(IAgent agent)
        {
            if (!agents.ContainsKey(agent))
                return new Maybe<IAgent>();

            float closestSQDistance = float.MaxValue;
            IAgent closest = null;

            foreach (var enemy in agents.Where(a => a.Key.IsActive && a.Value.Type == AgentType.Enemy && a.Key != agent))
            {
                var squaredDistance = (enemy.Value.GridLocation.x - agents[agent].GridLocation.x) ^ 2 +
                    (enemy.Value.GridLocation.y - agents[agent].GridLocation.y) ^ 2;
                if (squaredDistance < closestSQDistance)
                {
                    closest = enemy.Key;
                    closestSQDistance = squaredDistance;
                }
            }

            return closest != null? new Maybe<IAgent>(closest): new Maybe<IAgent>();
        }

        public Maybe<IAgent> GetTargetOf(IAgent agent)
        {
            return agents[agent].Target != null ? new Maybe<IAgent>(agents[agent].Target) : new Maybe<IAgent>(); 
        }
    }
}
