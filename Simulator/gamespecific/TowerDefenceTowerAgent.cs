using Simulator.actioncommands;
using Simulator.state;

namespace Simulator.gamespecific
{
    class TowerDefenceTowerAgent : IAgentType
    {
        public bool IsEnemy => false;

        public IActionGenerator GetLegalActionGenerator(IMapLayout map, IStateObject stateObject)
        {
            return new NoActionGenerator();
        }
    }
}
