using System.Collections.Generic;
using System.Linq;

namespace Simulator.actioncommands
{
    class CompositeMoveGenerator : IActionGenerator
    {
        private readonly IEnumerable<IActionGenerator> actionGenerators;

        public CompositeMoveGenerator(params IActionGenerator[] actionGenerators)
        {
            this.actionGenerators = actionGenerators;
        }

        public IEnumerable<IAction> Generate()
        {
            return actionGenerators.SelectMany(g => g.Generate());
        }
    }
}
