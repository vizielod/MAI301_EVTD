using Simulator.actioncommands;
using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviorTree
{
    class Rotate: LeafNode
    {
        public Rotate(string name, Blackboard blackboard) : base(name, blackboard)
        { }

        public override bool CheckConditions()
        {
            return true;
        }

        public override void DoAction()
        {
            blackboard.ChoosenAction = new Turn();
            controller.FinishWithSuccess();
        }

        public override void End()
        {
            LogTask("Ending");
        }

        public override void LogTask(string log)
        {
            Console.WriteLine("Name: " + name + ", " + log);
        }

        public override void Start()
        {
            LogTask("Starting");
        }
    }
}

