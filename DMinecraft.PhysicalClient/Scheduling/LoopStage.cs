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

        public TimeSpan Frequency { get; set; }

        public event Action<TimeSpan> Callback;

        public void OnCallback(TimeSpan deltaTime)
        {
            Callback?.Invoke(deltaTime);
        }
    }
}
