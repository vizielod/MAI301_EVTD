namespace Simulator.state
{
    enum AgentType
    {
        Enemy,
        Tower
    }

    class StateObject : IStateObject
    {
        public AgentType Type { get; set; }
        public bool IsActive { get; set; }
        public (int x, int y) GridLocation => (x, y);
        public IAgent Target { get; set; }
        public bool GoalReached { get; set; }

        private int x;
        private int y;

        public StateObject((int x, int y) gridLocation)
        {
            (x, y) = gridLocation;
            GoalReached = false;
        }

        public void Move(int xref, int yref)
        {
            x += xref;
            y += yref;
        }
    }
}
