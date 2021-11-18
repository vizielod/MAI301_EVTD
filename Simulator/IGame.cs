using System.Collections.Generic;

namespace Simulator
{
    internal interface IGame
    {
        IEnumerable<IAgent> Agents { get; }
        IState GenerateState();
        IStateObject GetStateObject(IAgent agent);
        void SpawnAgents(int round);
        void DespawnAgents(int round);
    }
}