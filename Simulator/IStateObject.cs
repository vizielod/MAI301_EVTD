namespace Simulator
{
    public interface IStateObject
    {
        bool IsActive { get; set; }
        (int x, int y) GridLocation { get; }
        IAgent Target { get; set; }

        void Move(int xref, int yref);
    }
}