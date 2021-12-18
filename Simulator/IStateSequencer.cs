using System.Collections.Generic;

namespace Simulator
{
    public interface IStateSequence
    {
        void StepForward();
        void StepBackward();
        IState GetCurrentStep();
        IEnumerable<IAgent> AllAgents { get; }
        IEnumerable<IAgent> AllEnemyAgents { get; }
        bool IsGameOver { get; }
        int NumberOfRounds { get; }
        void ReWind();
        IReadOnlyDictionary<IAgent, float> GetScores();
    }
}
