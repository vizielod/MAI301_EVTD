namespace Simulator.actioncommands
{
    public sealed class ScorePoints : IAction
    {
        void IAction.Apply(IStateObject stateObject)
        {
            stateObject.GoalReached = true;
            stateObject.IsEnabled = false;
        }

        void IAction.Undo(IStateObject stateObject)
        {
            stateObject.GoalReached = false;
            stateObject.IsEnabled = true;
        }
    }
}
