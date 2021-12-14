namespace Simulator
{
    public interface IStateObject
    {
        bool Spawned { get; set; }
        bool IsEnabled { get; set; }
        (int x, int y) GridLocation { get; }
        IAgent Target { get; set; }
        bool EngagedTarget { get; set; }
        void Move(int xref, int yref);
        bool GoalReached { get; set; }
        bool HasMoved { get; set; }
    }
}