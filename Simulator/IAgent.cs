using System.Collections.Generic;

namespace Simulator
{
    public interface IAgent
    {
        IAction PickAction();
    }
}