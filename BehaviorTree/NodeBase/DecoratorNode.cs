using System;
using System.Collections.Generic;

namespace BehaviorTree.NodeBase
{
    abstract class DecoratorNode:Node
    {
        public override NodeType Type => NodeType.Decorator;

        /**
         * Reference to the decorated node
         */
        protected Node node;

        public DecoratorNode(Node node)
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

        public override int Count()
        {
            return base.Count() + this.node.Count();
        }
    }
}
