namespace Simulator
{
    class WinConditionChain : IWinCondition
    {
        private readonly IWinCondition winCondition;
        private readonly IWinCondition successor;

        public WinConditionChain(IWinCondition winCondition, IWinCondition successor)
        {
            this.winCondition = winCondition;
            this.successor = successor;
        }

        public Alliances? GetWinner(int round)
        {
            Alliances? winner = winCondition.GetWinner(round);
            if (winner.HasValue)
                return winner;
            return successor.GetWinner(round);
        }
    }
}
