using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Scheduling.Coroutines.Generic
{
    internal interface ITimedCoroutine<T> : ITimedCoroutine
    {
        public T Result { get; }
    }
}
