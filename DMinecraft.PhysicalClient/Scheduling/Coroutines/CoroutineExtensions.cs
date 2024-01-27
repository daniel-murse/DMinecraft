using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Scheduling.Coroutines
{
    internal static class CoroutineExtensions
    {
        public static void Complete(this ICoroutine coroutine)
        {
            while (!coroutine.IsCompleted)
            {
                coroutine.Step();
            }
        }

        public static void Complete(this ITimedCoroutine coroutine)
        {
            while (!coroutine.IsCompleted)
            {
                coroutine.Step(TimeSpan.MaxValue);
            }
        }
    }
}
