using System.Collections.Generic;

namespace Simulator
{
    internal interface IGame
    {
        IEnumerable<IAgent> ActiveAgents { get; }
        IEnumerable<IAgent> AllAgents { get; }
        IEnumerable<IAgent> AllEnemyAgents { get; }
        int RoundLimit { get; }
        int PlayerLifes { get; }
        IEnumerable<IAgent> SuccessfulEnemies { get; }

        IState GenerateState();
        IStateObject GetStateObject(IAgent agent);
        void SpawnAgents(int round);
        int CountUnspawnedEnemies(int round);
        void NewRound();
        void DespawnAgents(int round);
        int CountEnemies();
        int CountActiveEnemies();
        int CountEnemiesSuccess();
        void Disable(IAgent agent);
        float GetProgression(IAgent agent);
        void VerifyPositions();
        bool IsActive(IAgent agent);
        void Reset();
    }
}