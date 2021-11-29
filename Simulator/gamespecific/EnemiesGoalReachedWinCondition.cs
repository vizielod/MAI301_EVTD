﻿namespace Simulator.gamespecific
{
    class EnemiesGoalReachedWinCondition : IWinCondition
    {
        private readonly IGame game;

        public EnemiesGoalReachedWinCondition(IGame game)
        {
            this.game = game;
        }

        public Alliances? GetWinner(int round)
        {
            if (game.CountEnemiesSuccess() == game.CountEnemies())
                return Alliances.Enemies;
            return null;
        }
    }
}