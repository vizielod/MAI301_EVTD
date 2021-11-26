using System;
using System.Linq;

namespace BehaviorTree.NodeBase
{
    public abstract class ParentNode:Node
    {
        protected ParentNodeController controller;

        public ParentNode(Blackboard blackboard):base(blackboard)
        {
            CreateController();
        }

        private void CreateController()
        {
            this.controller = new ParentNodeController();
        }

        public override NodeController GetControl()
        {
            return this.controller;
        }

        public abstract void AddChildren(Node node);

        /**
        * Abstract to be overridden in child
        * classes. Called when a child finishes
        * with success.
        */
        public abstract void ChildSucceeded();

        /**
        * Abstract to be overridden in child
        * classes. Called when a child finishes
        * with failure.
        */
        public abstract void ChildFailed();

        /**
        * Checks for the appropiate pre-state
        * of the data
        */
        public override bool CheckConditions()
        {
            return controller.subnodes.Count > 0;
        }


        public override void DoAction()
        {
            if (controller.Finished())
            {
                // If this parent task is finished
                // return without doing naught.
                return;
            }
            if (controller.currentNode == null)
            {
                // If there is a null child task
                // selected we've done something wrong
                return;
            }
            // If we do have a curTask...
            if (!controller.currentNode.
            GetControl().Started())
            {
                // ... and it's not started yet, start it.
                controller.currentNode.Start();
            }
            else if (controller.currentNode.
            GetControl().Finished())
            {
                // ... and it's finished, end it properly.
                controller.currentNode.End();
                if (controller.currentNode.
                GetControl().Succeeded())
                {
                    this.ChildSucceeded();
                }
                if (controller.currentNode.
                GetControl().Failed())
                {
                    this.ChildFailed();
                }
            }
            else
            {
                // ... and it's ready, update it.
                controller.currentNode.DoAction();
            }
        }

        /**
        * Ends the task
        */
        public override void End()
        {
            controller.SafeEnd();
        }

        /**
        * Starts the task, and points the
        * current task to the first one
        * of the available child tasks.
        */
        public override void Start()
        {
            controller.currentNode =
            controller.subnodes.First();
            if (controller.currentNode == null)
            {
                Console.Error.Write("Current task has a null action");
            }

            controller.SafeStart();
        }

        public override bool Running()
        {
            return !controller.Finished();
        }
    }
}
