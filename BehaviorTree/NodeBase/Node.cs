﻿using System;
namespace BehaviorTree.NodeBase
{
    /**
     * Base abstract class for all the nodes
     */
    public abstract class Node
    {
        protected Blackboard blackboard;

        /** 
         * When we create a new node we have to pass
         *the blackboard 
         */
        public Node(Blackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        /** 
         * Pre-condition check to see if
         * task can be updated
         */
        public abstract bool CheckConditions();

        /**
         * Override to add start-up logic
         */
        public abstract void Start();

        /**
         * Override to add ending logic
         */
        public abstract void End();

        public abstract bool Running();

        /**
         * Override to specify the logic that
         * the node should perform 
         * during each update
         */
        public abstract void DoAction();

        /**
         * Override to get the controller
         * assigned to the node
         */
        public abstract NodeController GetControl();
    }
}
