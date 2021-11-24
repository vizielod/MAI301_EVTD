using Simulator.actioncommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorTree
{
    class EnemyScore : LeafNode
    {
        public EnemyScore(string name, Blackboard bb):base(name, bb) { }
        public override bool CheckConditions()
        {
            return blackboard.LegalActions != null && blackboard.LegalActions.Any();
        }

        public override void DoAction()
        {
            LogTask("Doing action");

            if (blackboard.LegalActions.Any(a => a is ScorePoints))
            {
                blackboard.ChoosenAction = blackboard.LegalActions.First(a => a is ScorePoints);
                controller.FinishWithSuccess();
            }
            else
            {
                controller.FinishWithFailure();
            }
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
