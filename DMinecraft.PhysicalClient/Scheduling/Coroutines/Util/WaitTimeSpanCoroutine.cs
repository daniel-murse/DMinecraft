using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Scheduling.Coroutines.Util
{
    /// <summary>
    /// Represents a coroutine that will complete after the specified amount of time, after
    /// it is first stepped. Keep in mind that just because the amount of time specifed passed does not
    /// mean that the coroutine is finished; <see cref="Step"/> has to be called for <see cref="IsCompleted"/>, 
    /// to be <c>true</c>, as it is implemented using an enumerator. Is is true, however, 
    /// that this simple coroutine could be implemented without an enumerator, with merely a stopwatch.
    /// It is to note that in general, the execution of coroutines is at the mercy of users, and thats
    /// exactly the idea behind them; an operation is split up in increments that 
    /// </summary>
    internal class WaitTimeSpanCoroutine : ICoroutine
    {
        private IEnumerator WaitTimeSpanEnumerator(TimeSpan timeSpan)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (stopwatch.Elapsed < timeSpan) 
            {
                yield return null;
            }
        }

        private IEnumerator enumerator;

        public WaitTimeSpanCoroutine(TimeSpan timeSpan)
        {
            enumerator = WaitTimeSpanEnumerator(timeSpan);
        }

        public bool IsCompleted { get; private set; }

        public void Step()
        {
            IsCompleted = IsCompleted || !enumerator.MoveNext();
        }
    }
}
