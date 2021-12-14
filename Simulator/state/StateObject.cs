namespace Simulator.state
{
    class StateObject : IStateObject
    {
        public IAgentType Type { get; set; }
        public (int x, int y) GridLocation => (x, y);
        public IAgent Target { get; set; }
        public bool GoalReached { get; set; }

        public bool IsEnemy => Type.IsEnemy;

        public bool EngagedTarget { get; set; }
        public bool Spawned { get; set; }
        public bool IsEnabled { get; set; }

        private int x;
        private int y;

        public StateObject((int x, int y) gridLocation)
        {
            (x, y) = gridLocation;
            GoalReached = false;
            Spawned = false;
            IsEnabled = true;
        }

        public void Move(int xref, int yref)
        {
            x += xref;
            y += yref;
        }

        internal IActionGenerator GetLegalActionGenerator(IMapLayout map)
        {
            return Type.GetLegalActionGenerator(map, this);
        }
    }
}
