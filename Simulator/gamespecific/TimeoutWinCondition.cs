using System;
using System.Collections.Generic;
using System.Text;

namespace Simulator.gamespecific
{

    class TimeoutWinCondition : IWinCondition
    {
        private readonly int maxRounds;

        public TimeoutWinCondition(int maxRounds)
        {
            this.maxRounds = maxRounds;
        }
        public Alliances? GetWinner(int round)
        {
            if (round >= maxRounds) 
            {
                return Alliances.Player;
            }
            return null;
        }
    }
}
