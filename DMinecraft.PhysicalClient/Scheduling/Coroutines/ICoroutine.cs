using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Scheduling.Coroutines
{
    /// <summary>
    /// Represents a way to split up the execution of a task in multiple increments, "steps".
    /// Such a functionality can be useful in multiple cases, such as
    /// to easily design operations that we wish to split the execution of across multiple frames, like
    /// loading a lot of data (to avoid a large frame time) or even game logic.
    /// 
    /// Represents a coroutine that takes no parameters and returns no data.
    /// </summary>
    internal interface ICoroutine
    {
        public bool IsCompleted { get; }

        public void Step();
    }
}
