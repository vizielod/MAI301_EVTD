namespace Simulator.state
{
    internal class Event
    {
        public Event(IAgent agent, IAction action)
        {
            Agent = agent;
            Action = action;
        }

        public IAgent Agent { get; }
        public IAction Action { get; }
        public float Reward { get; }
    }
}