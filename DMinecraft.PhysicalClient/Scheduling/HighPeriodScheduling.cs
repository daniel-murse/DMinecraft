using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Scheduling
{
    /// <summary>
    /// Attempts to start a high performance (thread) scheduling session with the OS.
    /// </summary>
    internal class HighPeriodScheduling : IDisposable
    {
        public HighPeriodScheduling()
        {
            EnsureWinmmLoaded();
            TimeDevCaps timeDevCaps;
            if(timeGetDevCaps(out timeDevCaps, (uint)Unsafe.SizeOf<TimeDevCaps>()) != MMSYSERR_NOERROR)
                throw new PlatformNotSupportedException();
            if (timeBeginPeriod(timeDevCaps.wPeriodMin) != TIMER_NOERROR)
                throw new PlatformNotSupportedException();
            PeriodMilliSeconds = timeDevCaps.wPeriodMin;
        }

        #region Windows thread sleep extension
        [DllImport("winmm")]
        private static extern uint timeBeginPeriod(uint uPeriod);

        [DllImport("winmm")]
        private static extern uint timeEndPeriod(uint uPeriod);

        [DllImport("winmm")]
        private static extern uint timeGetDevCaps(out TimeDevCaps timeDevCaps, uint sizeBytes);

        private const uint TIMER_NOERROR = 0;

        private const uint MMSYSERR_NOERROR = 0;
        #endregion

        private bool disposedValue;

        public uint PeriodMilliSeconds { get; }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                //the obj wouldnt be created if this wasnt enabled (we throw in cstrcr)
                //so we can call it
                timeEndPeriod(PeriodMilliSeconds);
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~SchedulingPeriod()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private void EnsureWinmmLoaded()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ||
                timeBeginPeriod == null || timeEndPeriod == null || timeGetDevCaps == null)
            {
                throw new PlatformNotSupportedException();
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct TimeDevCaps
        {
            public uint wPeriodMin;
            public uint wPeriodMax;
        }
    }
}
