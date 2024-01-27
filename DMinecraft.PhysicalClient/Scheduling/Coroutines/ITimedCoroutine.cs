using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Scheduling.Coroutines
{
    /// <summary>
    /// Represents a coroutine that can be hinted to step for a specified amount of time.
    /// </summary>
    internal interface ITimedCoroutine
    {
        public bool IsCompleted { get; }

        public void Step(TimeSpan maxTimeHint);
    }
}
