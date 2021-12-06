using System;

namespace BehaviorTree.NodeBase
{
    abstract class LeafNode : Node
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

        public override void DoAction(Blackboard blackboard)
        {
            blackboard.AcceptVisitor(this);
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

        public abstract void HandleEnemy(EnemyBlackboard blackboard);

        public abstract void HandleTurret(TurretBlackboard blackboard);
    }
}
