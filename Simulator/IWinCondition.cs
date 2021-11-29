namespace Simulator
{
    interface IWinCondition
    {
        Alliances? GetWinner(int round);
    }
}
