using Simulator;
using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviorTree.Agents
{
    public interface IEnemyAgent : IAgent
    {
        int Health { get; }
    }
}
