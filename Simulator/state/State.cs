using Simulator.actioncommands;
using Simulator.gamespecific;
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
        private readonly BreadthFirstSearch bfs;
        public Alliances? Winner { get; set; }

        public int ScoredPoints { get; set; }

        public State(IMapLayout map, BreadthFirstSearch bfs)
        {
            MapLayout = map;
            this.bfs = bfs;
            agents = new Dictionary<IAgent, StateObject>();
        }

        public void AddAgent(IAgent agent, (int x, int y) gridLocation, IAgentType type)
        {
            agents.Add(agent, new StateObject(gridLocation)
            {
                Type = type
            });
        }

        public IEnumerable<IAgent> GetAgentsAt(int x, int y)
        {
            return agents.Where(p => p.Value.GridLocation == (x, y)).Select(p => p.Key);            
        }

        public (int x, int y) PositionOf(IAgent agent)
        {
            if (agents.ContainsKey(agent))
                return agents[agent].GridLocation;
            return (0, 0);
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
                var squaredDistance = Math.Pow(enemy.Value.GridLocation.x - agents[agent].GridLocation.x, 2) + Math.Pow(enemy.Value.GridLocation.y - agents[agent].GridLocation.y, 2);

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
        public bool EngagedTargetOf(IAgent agent)
        {
            return agents[agent].Target != null && agents[agent].EngagedTarget;
        }

        public Maybe<(int x, int y)> SuggestPosition(IAgent agent)
        {
            return Maybe.Create(bfs.Next(agents[agent].GridLocation));
        }

        internal void SetTarget(IAgent agent, IAgent target, bool engaged = true)
        {
            if (target != null && agents.ContainsKey(target))
            {
                agents[agent].Target = target;
                agents[agent].EngagedTarget = engaged;
            }
        }

        public Maybe<IAgent> GetClosestTurret(IAgent agent)
        {
            if (!agents.ContainsKey(agent))
                return new Maybe<IAgent>();

            double closestSQDistance = double.MaxValue;
            IAgent closest = null;

            foreach (var turret in agents.Where(a => (a.Key.IsActive && !a.Value.Type.IsEnemy)))
            {
                var squaredDistance = Math.Pow(turret.Value.GridLocation.x - agents[agent].GridLocation.x, 2) + Math.Pow(turret.Value.GridLocation.y - agents[agent].GridLocation.y, 2);

                if (squaredDistance < closestSQDistance)
                {
                    closest = turret.Key;
                    closestSQDistance = squaredDistance;
                }
            }

            return Maybe.Create(closest);
        }

        public IEnumerable<IAgent> GetTurretsAttacking(IAgent agent)
        {
            return agents.Where(a => a.Value.Target == agent && a.Value.EngagedTarget).Select(a => a.Key);
        }

        public Direction GetDirection(IAgent from, IAgent to)
        {
            var (fromX, fromY) = PositionOf(from);
            var (toX, toY) = PositionOf(to);

            return MapLayout.Translate(toX - fromX, toY - fromY);
        }

        public IAction SuggestedAction(IAgent agent)
        {
            return SuggestPosition(agent).ApplyOrDefault(pos =>
            {
                var (x, y) = pos;
                if (x == -1 && y == -1)
                {
                    return new ScorePoints();
                }
                return new RigidMoveGenerator(MapLayout, agents[agent]).Translate(x,y);
            }, new Idle());
        }

        public IReadOnlyDictionary<Direction, int> GetWallDistances(IAgent agent)
        {
            var result = new Dictionary<Direction, int>();

            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                int counter = -1;

                var (xRel,yRel) = MapLayout.Translate(direction);
                var (x, y) = PositionOf(agent);

                do
                {
                    x += xRel;
                    y += yRel;
                    counter++;
                }
                while (MapLayout.InBounds(x, y) && MapLayout.TypeAt(x,y) != TileType.Wall);

                result.Add(direction, counter);
            }
            return result;
        }

        public bool IsActive(IAgent agent)
        {
            return agents.ContainsKey(agent) && agent.IsActive;
        }

        public bool HasScored(IAgent agent)
        {
            return agents.ContainsKey(agent) && agents[agent].GoalReached;
        }
    }
}
