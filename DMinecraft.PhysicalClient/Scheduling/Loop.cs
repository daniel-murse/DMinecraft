using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Scheduling
{
    internal class Loop
    {
        private LoopStageTimer[] timers;

        public bool IsRunning { get; set; }

        public TimeSpan SleepError { get; set; }

        public bool DoSleeping { get; set; }

        private DateTime nextStageTime;

        public Loop(IEnumerable<LoopStage> stages)
        {
            IsRunning = true;
            timers = stages.Select(p => new LoopStageTimer(p)).ToArray();
        }

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
