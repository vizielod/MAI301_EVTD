using System;
namespace BehaviorTree
{
    public static class ClampHelper
    {
        public static float Clamp(this float value, float min, float max)
        {
            return value > max ? max :
                   value < min ? min :
                   value;
        }
    }
}
