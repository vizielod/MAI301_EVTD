using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviorTree.Agents
{
    public interface IAdaptiveEnemy : IEnemyAgent
    {
        void Reset();
        IEnemyAgent Clone();
        AgentBuilder ReverseEngineer();
    }
}
