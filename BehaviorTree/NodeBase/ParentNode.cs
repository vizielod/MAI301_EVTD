using System;
using System.Collections.Generic;
using System.Linq;

namespace BehaviorTree.NodeBase
{
    abstract class ParentNode : Node
    {
        protected ParentNodeController controller;

        public ParentNode()
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

        public void AddChildren(Node node)
        {
            controller.AddNode(node);
        }

        /**
        * Abstract to be overridden in child
        * classes. Called when a child finishes
        * with success.
        */
        public abstract void ChildSucceeded();

        public abstract void ChildIsRunning();

        internal void RemoveChild(Node child)
        {
            controller.subnodes.Remove(child);
        }

        internal int IndexOf(Node child)
        {
            return controller.subnodes.IndexOf(child);
        }

        /**
        * Abstract to be overridden in child
        * classes. Called when a child finishes
        * with failure.
        */
        public abstract void ChildFailed();

        internal void Insert(int index, Node child)
        {
            controller.subnodes.Insert(index, child);
        }

        /**
        * Checks for the appropiate pre-state
        * of the data
        */
        public override bool CheckConditions()
        {
            return controller.subnodes.Count > 0;
        }


        public override void DoAction(Blackboard blackboard)
        {
            if (controller.subnodes.Count() == 0)
            {
                controller.FinishWithFailure();
                return;
            }

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
                if (controller.currentNode.GetControl().Running())
                {
                    this.ChildIsRunning();
                }
                else if(controller.currentNode.
                GetControl().Succeeded())
                {
                    // ... and it's finished, end it properly.
                    controller.currentNode.End();
                    this.ChildSucceeded();
                    
                }
                else if (controller.currentNode.
                GetControl().Failed())
                {
                    // ... and it's finished, end it properly.
                    controller.currentNode.End();
                    this.ChildFailed();
                } 
            }
            else
            {
                // ... and it's ready, update it.
                controller.currentNode.DoAction(blackboard);
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
            if (!controller.Running())
            {
                controller.currentNode =
            controller.subnodes.First();
            }
            else
            {
                controller.currentNode.GetControl().Reset();
            }

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

        internal IEnumerable<LeafNode> GetAllLeafNodes()
        {
            foreach (var child in controller.subnodes)
            {
                if (child is ParentNode composit)
                {
                    foreach (var leaf in composit.GetAllLeafNodes())
                        yield return leaf;
                }

                if (child is LeafNode leafNode)
                    yield return leafNode;
            }
        }

        public override int Count()
        {
            return base.Count() + controller.subnodes.Sum(c => c.Count());
        }
    }
}
