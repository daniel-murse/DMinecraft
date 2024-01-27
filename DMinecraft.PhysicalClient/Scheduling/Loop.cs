using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Scheduling
{
    /// <summary>
    /// Represents a loop consisting of different callbacks that must be run at different
    /// intervals. Supports sleeping.
    /// </summary>
    internal class Loop
    {
        private LoopStageTimer[] timers;

        /// <summary>
        /// Set to false to stop the loop.
        /// </summary>
        public bool IsRunning { get; set; }

        /// <summary>
        /// Substracted from the time left to sleep to account for scheduling errors.
        /// </summary>
        public TimeSpan SleepError { get; set; }

        /// <summary>
        /// If sleeping should be done, or busy waiting instead.
        /// </summary>
        public bool DoSleeping { get; set; }

        private DateTime nextStageTime;

        /// <summary>
        /// Constructs a loop with the specified callbacks in order.
        /// </summary>
        /// <param name="stages">The callbacks, in order.</param>
        public Loop(IEnumerable<LoopStage> stages)
        {
            IsRunning = true;
            timers = stages.Select(p => new LoopStageTimer(p)).ToArray();
        }

        /// <summary>
        /// Runs the loop synchronously until <see cref="IsRunning"/> is set to false.
        /// Stages are run if their time has come or its past their time.
        /// Handles sleeping too, based on the time of the next scheduled
        /// callback.
        /// </summary>
        public void Run()
        {
            while (IsRunning)
            {
                nextStageTime = DateTime.MaxValue;
                foreach (var timer in timers)
                {
                    nextStageTime = new DateTime(Math.Min(timer.OnLoopIteration().Ticks, nextStageTime.Ticks));
                    if (!IsRunning)
                        break;
                }

                if(DoSleeping)
                {
                    TimeSpan sleepAmount = nextStageTime - DateTime.Now - SleepError;
                    //CARE this rounds to milliseconds (most likely) and it should, due to the
                    //os scheduling timer period, which wmme can specify with a 1ms accuracy
                    if (sleepAmount > TimeSpan.Zero)
                        Thread.Sleep(sleepAmount);
                }
            }
        }

        private class LoopStageTimer
        {
            public LoopStageTimer(LoopStage stage)
            {
                this.stage = stage ?? throw new ArgumentNullException();
                stopwatch = new Stopwatch();
            }

            private LoopStage stage;

            private Stopwatch stopwatch;

            public DateTime OnLoopIteration()
            {
                if(!stopwatch.IsRunning)
                {
                    stopwatch.Start();
                    stage.OnCallback(TimeSpan.Zero);
                }
                else
                {
                    if(stopwatch.Elapsed > stage.Frequency)
                    {
                        stage.OnCallback(stopwatch.Elapsed);
                        stopwatch.Restart();
                    }
                }
                return DateTime.Now + stage.Frequency - stopwatch.Elapsed;
            }
        }
    }
}
