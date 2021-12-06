namespace Simulator
{
    public interface IAgent
    {
        IAction PickAction(IState state);
        (int x, int y) InitialPosition { get; }
        int SpawnRound { get; }
        void Damage(int v);
        void Heal(int v);
        bool IsActive { get; }
        bool IsEnemy { get; }
        float HealthRatio { get; }
    }
}