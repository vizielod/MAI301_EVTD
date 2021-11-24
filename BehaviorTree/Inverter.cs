using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviorTree
{
    public class Inverter : DecoratorNode
    {
        public Inverter(string name, Blackboard blackboard, Node node) : base(name, blackboard, node)
        {
        }

        public override void DoAction()
        {
            node.DoAction();

            if (node.GetControl().Succeeded())
            {
                GetControl().FinishWithFailure();
            }
            else 
            {
                GetControl().FinishWithSuccess();
            }
        }

        public override void LogTask(string log)
        {
            Console.WriteLine("Name: " + name + ", " + log);
        }
    }
}
