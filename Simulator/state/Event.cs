namespace Simulator.state
{
    internal class Event
    {
        public IAgent Agent { get; }
        public IAction Action { get; }
        public float Reward { get; }
    }
}