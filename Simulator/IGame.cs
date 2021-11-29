using System.Collections.Generic;

namespace Simulator
{
    internal interface IGame
    {
        IEnumerable<IAgent> ActiveAgents { get; }
        IEnumerable<IAgent> AllAgents { get; }
        IState GenerateState();
        IStateObject GetStateObject(IAgent agent);
        void SpawnAgents(int round);
        int CountUnspawnedEnemies(int round);
        void DespawnAgents(int round);
        int CountEnemies();
        int CountActiveEnemies();
        int CountEnemiesSuccess();
    }
}