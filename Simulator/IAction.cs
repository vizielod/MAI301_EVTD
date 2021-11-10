namespace Simulator
{
    public enum Direction
    {
        North,
        South,
        East,
        West
    }

    public interface IAction
    {
        void Apply();
        void Undo();
        Direction GetDirection();
    }
}