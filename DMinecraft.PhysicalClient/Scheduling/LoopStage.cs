using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Scheduling
{
    internal class LoopStage
    {
        public LoopStage(TimeSpan frequency, Action<TimeSpan> callback)
        {
            Frequency = frequency;
            Callback += callback;
        }

        /// <summary>
        /// The frequency at which the callback should be invoked.
        /// </summary>
        public TimeSpan Frequency { get; set; }

        /// <summary>
        /// The callbacks to invoke.
        /// </summary>
        public event Action<TimeSpan> Callback;

        /// <summary>
        /// Invokes the callbacks.
        /// </summary>
        /// <param name="deltaTime">The time since the last invocation.</param>
        public void OnCallback(TimeSpan deltaTime)
        {
            Callback?.Invoke(deltaTime);
        }
    }
}
