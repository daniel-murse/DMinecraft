using DMinecraft.PhysicalClient.Scheduling.Coroutines.Generic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Scheduling.Coroutines
{
    internal class SequentialCoroutinesCoroutine : ITimedCoroutine
    {
        private IEnumerator<ITimedCoroutine> coroutines;

        private ITimedCoroutine? current;

        public SequentialCoroutinesCoroutine(IEnumerator<ITimedCoroutine> coroutines)
        {
            this.coroutines = coroutines;
        }

        public bool IsCompleted { get; set; }

        public void Step(TimeSpan maxTimeHint)
        {
            if(current == null && !coroutines.MoveNext())
            {
                IsCompleted = true;
                return;
            }

            current = coroutines.Current;
            current.Step(maxTimeHint);
            
            if(current.IsCompleted)
            {
                current = null;
            }
        }
    }
}
