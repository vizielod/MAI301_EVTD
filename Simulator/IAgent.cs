using System.Collections.Generic;

namespace Simulator
{
    public interface IAgent
    {
        IAction PickAction(IState state);
    }
}