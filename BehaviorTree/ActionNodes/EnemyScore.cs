using BehaviorTree.NodeBase;
using Simulator.actioncommands;
using System.Linq;

namespace BehaviorTree.ActionNodes
{
    class EnemyScore : LeafNode
    {
        public EnemyScore( Blackboard bb):base( bb) { }
        public override bool CheckConditions()
        {
            return blackboard.LegalActions != null && blackboard.LegalActions.Any();
        }

        public override void DoAction()
        {

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
    }
}
