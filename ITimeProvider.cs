using Raylib_cs;
using System.Collections.Generic;
using System.Numerics;

namespace CustomProgram
{
    public interface ITimeProvider
    {
        double GetCurrentTime();
    }

    public class RealTimeProvider : ITimeProvider
    {
        // This method returns the current time using Raylib's GetTime method.
        public double GetCurrentTime()
        {
            return Raylib.GetTime();
        }
    }
}
