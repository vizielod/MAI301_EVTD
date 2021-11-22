using Simulator.actioncommands;
using System.Collections.Generic;

namespace Simulator.gamespecific
{
    class ScoreActionGenerator : IActionGenerator
    {
        private readonly IMapLayout map;
        private readonly IStateObject agentState;

        public ScoreActionGenerator(IMapLayout map, IStateObject agentState)
        {
            this.map = map;
            this.agentState = agentState;
        }

        public IEnumerable<IAction> Generate()
        {
            (int x, int y) = agentState.GridLocation;
            if (map.TypeAt(x, y) == TileType.Goal)
                yield return new ScorePoints();
        }
    }
}
