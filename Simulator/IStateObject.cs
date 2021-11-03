namespace Simulator
{
    public interface IStateObject
    {
        (int x, int y) GridLocation { get; }
    }
}