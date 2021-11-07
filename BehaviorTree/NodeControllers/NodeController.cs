﻿using System;
namespace BehaviorTree
{
    public class NodeController
    {
        /**
        * Indicates if the task has started
        * or not
        */
        private bool started;

        /**
        * Indicates whether the task is finished
        * or not
        */
        private bool done;

        /**
        * If finished, it indicates if it has
        * finished with success or not
        */
        private bool sucess;

        /**
         * Reference to the node that
         * the controller controlls
         */
        private Node node;

        public NodeController(Node node)
        {
            SetNode(node);
            Initialize();
        }

        private void Initialize()
        {
            this.started = false;
            this.done = false;
            this.sucess = true;
        }

        /**
         * Set's the referenced task
         */
        public void SetNode(Node node)
        {
            this.node = node;
        }

        /**
         * Starts the monitored class
         */
        public void SafeStart()
        {
            this.started = true;

            node.Start();
        }

        /**
         * Ends the monitored class
         */
        public void SafeEnd()
        {
            this.started = false;
            this.done = false;
            
            node.End();
        }

        /**
        * Ends the monitored class, with success
        */
        public void FinishWithSuccess()
        {
            this.sucess = true;
            this.done = true;
            node.LogTask("Finished with success");
        }

        /**
        * Ends the monitored class, with failure
        */
        public void FinishWithFailure()
        {
            this.sucess = false;
            this.done = true;
            node.LogTask("Finished with failure");
        }

        /**
        * Marks the class as just started.
        */
        public void Reset()
        {
            this.done = false;
        }

        /**
         * Getters
         */

        public bool Started()
        {
            return this.started;
        }

        public bool Finished()
        {
            return this.done;
        }

        public bool Succeeded()
        {
            return this.sucess;
        }

        public bool Failed()
        {
            return !this.sucess;
        }

    }
}
