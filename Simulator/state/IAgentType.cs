namespace Simulator.state
{
    interface IAgentType
    {
        bool IsEnemy { get; }
        IActionGenerator GetLegalActionGenerator(IMapLayout map, IStateObject stateObject);
    }
}