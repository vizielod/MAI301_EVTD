using System.Linq;

namespace Simulator.gamespecific
{
    class EnemiesDefeatedWinCondition : IWinCondition
    {
        private readonly IGame game;

        public EnemiesDefeatedWinCondition(IGame game)
        {
            this.game = game;
        }

        public Alliances? GetWinner(int round)
        {
            if (game.CountUnspawnedEnemies(round) > 0)
                return null;

            if (game.CountActiveEnemies() == 0)
                return Alliances.Player;
            return null;
        }
    }
}
