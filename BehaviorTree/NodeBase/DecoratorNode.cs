using System;
namespace BehaviorTree
{
    public abstract class DecoratorNode:Node
    {
        /**
         * Reference to the decorated node
         */
        protected Node node;

        public DecoratorNode(Blackboard blackboard, Node node):base(blackboard)
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

        public override bool Running()
        {
            return GetControl().Finished();
        }
    }
}
