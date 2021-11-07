namespace BehaviorTree
{
    public abstract class LeafNode : Node
    {
        protected NodeController controller;

        public LeafNode(string name, Blackboard blackboard):base(name, blackboard)
        {
            CreateController();
        }

        private void CreateController()
        {
            this.controller = new NodeController(this);
        }

        public override NodeController GetControl()
        {
            return this.controller;
        }
    }
}
