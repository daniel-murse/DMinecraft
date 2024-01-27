using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Scheduling.Coroutines.Generic
{
    /// <summary>
    /// Represents a way to split up the execution of a task in multiple increments, "steps".
    /// Such a functionality can be useful in multiple cases, but for us it is of interest due to the ability
    /// to easily design operations that we wish to split the execution of across multiple frames, such
    /// as loading a lot of data (to avoid a large frame time) or even game logic.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal interface ICoroutine<T> : ICoroutine
    {
        public T Result { get; }
    }
}
