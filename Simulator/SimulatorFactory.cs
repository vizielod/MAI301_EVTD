using Simulator.state;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Simulator
{
    public class SimulatorFactory
    {
        public IStateSequence CreateSimulator(IMapLayout initialMap)
        {
            if (initialMap.Count(t => t == TileType.Goal) != 1)
                throw new ArgumentException("Initial map must have exactly one goal", nameof(initialMap));

            var state = new State(initialMap);
            (int x, int y) = initialMap.GetSpawnPoint();
            state.AddAgent(new DummyAgent(), x, y);
            return new Simulator(state);
        }

        class DummyAgent : IAgent
        {
            public IAction PickAction(IState state)
            {
                IEnumerable<IAction> actions = state.GetLegalActionGenerator(this).Generate();

                /*Blackboard bb = new Blackboard(actions);

                Selector move = new Selector("Selector", bb);
                ((ParentTaskController)move.GetControl()).
                 Add(new MoveEast(
                 "MoveEast", bb));
                ((ParentTaskController)move.GetControl()).
                 Add(new MoveSouth(
                 "MoveSouth", bb));
                ((ParentTaskController)move.GetControl()).
                 Add(new MoveWest(
                 "MoveWest", bb));
                ((ParentTaskController)move.GetControl()).
                 Add(new MoveNorth(
                 "MoveNorth", bb));

                ((ParentTaskController)move.GetControl()).SafeStart();

                while (!((ParentTaskController)move.GetControl()).Finished()) 
                {
                    move.DoAction();
                }

                ((ParentTaskController)move.GetControl()).SafeEnd();

                return bb.choosenAction;*/
                return state.GetLegalActionGenerator(this).Generate().First();

            }
        }
    }
}
