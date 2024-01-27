using DMinecraft.PhysicalClient.Content;
using DMinecraft.PhysicalClient.Scheduling.Coroutines;
using StbImageSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Shaders.Content
{
    /// <summary>
    /// Loads all shaders in a specified directory.
    /// </summary>
    internal class ShaderCacheCoroutine : ITimedCoroutine
    {
        public ShaderCacheCoroutine(ShaderCache shaderCache, string path)
        {
            Path = path;
            ShaderCache = shaderCache;
            filePaths = Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories).GetEnumerator();
            previousElapsed = TimeSpan.Zero;
            timer = new Stopwatch();
        }

        public string Path { get; }

        public ShaderCache ShaderCache { get; }

        private IEnumerator<string> filePaths;

        public bool IsCompleted { get; private set; }

        private TimeSpan previousElapsed;

        Stopwatch timer;

        public void Step(TimeSpan maxTimeHint)
        {
            if (!filePaths.MoveNext())
            {
                IsCompleted = true;
                return;
            }

            timer.Restart();

            do
            {
                var start = timer.Elapsed;
                string key = System.IO.Path.GetRelativePath(Path, filePaths.Current);
                //belongs here ig
                //keys use the / system, as decided now, so when creating keys from arbitrary data,
                //including files, it is the responsibilty of those to ensure the format is correct
                key = key.Replace("\\", "/");

                using (var reader = new StreamReader(File.OpenRead(filePaths.Current)))
                    ShaderCache.LoadShader(reader, key);

                previousElapsed = timer.Elapsed - start;
            } while (timer.Elapsed + previousElapsed < maxTimeHint && filePaths.MoveNext());
        }
    }
}
