namespace Simulator.gamespecific
{
    class EnemiesGoalReachedWinCondition : IWinCondition
    {
        private readonly IGame game;
        private readonly int target;

        public EnemiesGoalReachedWinCondition(IGame game, int target)
        {
            this.game = game;
            this.target = target;
        }

        public Alliances? GetWinner(int round)
        {
            if (game.CountEnemiesSuccess() >= target)
                return Alliances.Enemies;
            return null;
        }
    }
}
