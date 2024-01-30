using DMinecraft.PhysicalClient.Scenes;
using DMinecraft.PhysicalClient.Scenes.Init;
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
using OpenTK.Graphics.OpenGL4;

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
                this.settings = new AppSettings(args) { ContentRootPath = "./res/dminecraft/content/"};
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

            private IScene scene;

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
                else if (settings.EnableSleep)
                {
                    logger?.Warning("Sleep enabled without a high period timer.");
                }

                updateLoopStage = new LoopStage(settings.UpdateFrequency, this.OnUpdate);
                renderLoopStage = new LoopStage(settings.RenderFrequency, this.OnRender);

                loop = new Loop(new LoopStage[] {updateLoopStage, renderLoopStage});
                loop.SleepError = settings.SleepError;
                loop.DoSleeping = settings.EnableSleep;

                window.Resize += OnWindowResize;

                scene = new InitScene(new Graphics.OpenGL.GLObjects.GLContext("dminecraft"), new InitSceneSettings()
                { ContentRoot = "c:/users/danie/desktop/dminecraft/content" });
            }

            private void OnWindowResize(OpenTK.Windowing.Common.ResizeEventArgs obj)
            {
                GL.Viewport(0, 0, obj.Width, obj.Height);
            }

            private void OnRender(TimeSpan deltaTime)
            {
                scene.Render(deltaTime);
                window.Context.SwapBuffers();
            }

            private void OnUpdate(TimeSpan deltaTime)
            {
                //CARE glfw (the current impl) causes this to process events or all windows
                //and must be called from the main thread
                //ideally u change impl sometime, not even for functionality necessarily, but for
                //fun and code
                window.ProcessEvents();

                scene.Update(deltaTime);
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
