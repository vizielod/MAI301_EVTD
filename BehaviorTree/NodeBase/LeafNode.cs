using System;

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

        public override bool Running()
        {
            return !controller.Finished();
        }

        public override void End()
        {
            LogTask("Ending");
            controller.SafeEnd();
        }

        public override void LogTask(string log)
        {
            Console.WriteLine("Name: " + name + ", " + log);
        }

        public override void Start()
        {
            LogTask("Starting");
            controller.SafeStart();
        }
    }
}
