using DMinecraft.PhysicalClient.Scheduling;
using DMinecraft.PhysicalClient.Windowing;
using OpenTK.Windowing.Desktop;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace DMinecraft
{
    namespace PhysicalClient
    {
        /// <summary>
        /// The DMinecraft physical client.
        /// </summary>
        public class App : IDisposable
        {
            public App(string[] args)
            {
                this.settings = new AppSettings(args);
                this.window = new AppWindow(settings.WindowSettings);
                //todo use when a logger is passed
                //new LoggerConfiguration().WriteTo.Logger
                this.logger = new LoggerConfiguration()
                    .MinimumLevel.Is(settings.IsDebug ? LogEventLevel.Debug : LogEventLevel.Error)
                    .WriteTo.Console()
                    .CreateLogger();
            }

            private AppWindow window;

            private AppSettings settings;

            //private bool isRunning;

            private HighPeriodScheduling? schedulingPeriod;

            private ILogger logger;

            private Loop loop;

            private bool disposedValue;

            private LoopStage updateLoopStage;

            private LoopStage renderLoopStage;

            public void Run()
            {
                Start();

                loop.Run();

                Close();
            }

            private void Close()
            {
                Dispose();
            }

            private void Start()
            {

                if(settings.EnableHighPeriodTimer)
                {
                    try
                    {
                        schedulingPeriod = new HighPeriodScheduling();
                    }
                    catch (PlatformNotSupportedException)
                    {
                        logger?.Error("Could not enable a high period timer.");
                    }
                }

                updateLoopStage = new LoopStage(settings.UpdateFrequency, this.OnUpdate);
                renderLoopStage = new LoopStage(settings.RenderFrequency, this.OnRender);

                loop = new Loop(new LoopStage[] {updateLoopStage, renderLoopStage});
                loop.SleepError = settings.SleepError;
                loop.DoSleeping = settings.EnableSleep;
            }

            private void OnRender(TimeSpan deltaTime)
            {
                Console.WriteLine("Render");
            }

            private void OnUpdate(TimeSpan deltaTime)
            {
                Console.WriteLine("Update");
            }

            private void DisposeUnmanaged()
            {
                window.Dispose();
                schedulingPeriod?.Dispose();
            }

            protected virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        // TODO: dispose managed state (managed objects)
                    }
                    DisposeUnmanaged();
                    // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                    // TODO: set large fields to null
                    disposedValue = true;
                }
            }

            // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
            ~App()
            {
                // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
                Dispose(disposing: false);
            }

            public void Dispose()
            {
                // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
                Dispose(disposing: true);
                GC.SuppressFinalize(this);
            }
        }
    }
}
