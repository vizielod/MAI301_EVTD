namespace Simulator.state
{
    class StateObject : IStateObject
    {
        public IAgentType Type { get; set; }
        public bool IsActive { get; set; }
        public (int x, int y) GridLocation => (x, y);
        public IAgent Target { get; set; }
        public bool GoalReached { get; set; }

        public bool IsEnemy => Type.IsEnemy;

        public bool EngagedTarget { get; set; }

        private int x;
        private int y;

        public StateObject((int x, int y) gridLocation)
        {
            (x, y) = gridLocation;
            GoalReached = false;
            IsActive = false;
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
