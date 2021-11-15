namespace Simulator
{
    public interface IStateObject
    {
        bool IsActive { get; set; }
        (int x, int y) GridLocation { get; }
        void Move(int xref, int yref);
    }
}