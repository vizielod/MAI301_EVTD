using Simulator;

namespace Evolution
{
    public class GenerationInfo
    {
        public float Score { get; }
        public int Generation { get; }
        public IStateSequence Simulation { get; }

        public GenerationInfo(int generation, float score, IStateSequence simulation)
        {
            Generation = generation;
            Score = score;
            Simulation = simulation;
        }

    }
}
