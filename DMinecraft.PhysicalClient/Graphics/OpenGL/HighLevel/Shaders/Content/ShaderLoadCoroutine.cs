using DMinecraft.PhysicalClient.Content;
using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects;
using DMinecraft.PhysicalClient.Scheduling.Coroutines;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Shaders.Content
{
    //todo an async IContentSource (async stream acquiring too), such that its fully async
    //sync for now, since its not a real concern; the individual shader loads do not need to span
    //across frames, just the set of multiple loads does
    internal class ShaderLoadCoroutine : ITimedCoroutine, IDisposable
    {
        public GLShader? Shader { get; }

        public IContentSource ContentSource { get; }

        public bool IsCompleted { get; set; }

        private Task readTask;

        private StreamReader streamReader;

        private Stopwatch stopwatch;

        private TimeSpan previousElapsed;

        public ShaderLoadCoroutine(IContentSource contentSource, TimeSpan shaderCreateTime)
        {
            ContentSource = contentSource;
            stopwatch = new Stopwatch();
        }

        public void Step(TimeSpan maxTimeHint)
        {
            stopwatch.Restart();
            if (readTask == null)
            {
                streamReader = new StreamReader(ContentSource.OpenRead());
                readTask = streamReader.ReadToEndAsync();
            }

            while (stopwatch.Elapsed < maxTimeHint)
            {
                //busy wait
                if (readTask.IsCompleted)
                {

                }
            }
        }

        public void Dispose()
        {
            ((IDisposable)streamReader)?.Dispose();
        }
    }
}
