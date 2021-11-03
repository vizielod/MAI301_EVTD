namespace Simulator
{
    public interface IAction
    {
        void Apply();
        void Undo();
    }
}