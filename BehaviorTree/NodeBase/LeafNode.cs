﻿using System;

namespace BehaviorTree.NodeBase
{
    public abstract class LeafNode : Node
    {
        protected NodeController controller;

        public LeafNode()
        {
            CreateController();
        }

        private void CreateController()
        {
            this.controller = new NodeController();
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
            controller.SafeEnd();
        }

        public override void Start()
        {
            controller.SafeStart();
        }
    }
}
