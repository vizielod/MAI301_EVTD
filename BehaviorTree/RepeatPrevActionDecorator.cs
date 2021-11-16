using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorTree
{
    class RepeatPrevActionDecorator : DecoratorNode
    {

        public RepeatPrevActionDecorator(string name, Blackboard blackboard, Node node) : base(name, blackboard, node)
        { }
        public override void DoAction()
        {
            if (blackboard.legalActions.Contains(blackboard.previousAction))
            {
                blackboard.choosenAction = blackboard.previousAction;
                GetControl().FinishWithSuccess();
            }
            else 
            {
                node.DoAction();
            }
        }

        public override void LogTask(string log)
        {
            Console.WriteLine("Name: " + name + ", " + log);
        }
    }
}
