using System;
namespace BehaviorTree
{
    public abstract class DecoratorNode:Node
    {
        /**
         * Reference to the decorated node
         */
        Node node;

        public DecoratorNode(string name, Blackboard blackboard, Node node):base(name,blackboard)
        {
            InitTask(node);
        }

        private void InitTask(Node node)
        {
            this.node = node;
            this.node.GetControl().SetNode(this);
        }

        public override bool CheckConditions()
        {
            return this.node.CheckConditions();
        }

        public override void Start()
        {
            this.node.Start();
        }

        public override void End()
        {
            this.node.End();
        }

        public override NodeController GetControl()
        {
            return this.node.GetControl();
        }
    }
}
