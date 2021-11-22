using Simulator.actioncommands;
using Simulator.state;

namespace Simulator.gamespecific
{
    class TowerDefenceEnemyAgent : IAgentType
    {
        public bool IsEnemy => true;

        public IActionGenerator GetLegalActionGenerator(IMapLayout map, IStateObject stateObject)
        {
            return new CompositeMoveGenerator(
                new RigidMoveGenerator(map, stateObject), 
                new ScoreActionGenerator(map, stateObject)
                );
        }
    }
}
