namespace Simulator
{
    public interface IAgent
    {
        IAction PickAction(IState state);
        (int x, int y) InitialPosition { get; }
        int spawnRound { get; }
        void Damage(int v);
        void Heal(int v);
    }
}