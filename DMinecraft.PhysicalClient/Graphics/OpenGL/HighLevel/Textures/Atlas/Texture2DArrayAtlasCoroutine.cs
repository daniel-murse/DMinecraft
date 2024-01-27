using DMinecraft.PhysicalClient.Scheduling.Coroutines;
using OpenTK.Graphics.OpenGL4;
using StbImageSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Textures.Atlas
{
    internal class Texture2DArrayAtlasCoroutine : ITimedCoroutine
    {
        private string path;

        private Texture2DArrayAtlas atlas;

        private IEnumerator implementation;

        public Texture2DArrayAtlasCoroutine(Texture2DArrayAtlas atlas, string path)
        {
            this.path = path;
            this.atlas = atlas;
            filePaths = Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories).GetEnumerator();
            previousElapsed = TimeSpan.Zero;
            timer = new Stopwatch();
        }

        public bool IsCompleted { get; private set; }

        private TimeSpan previousElapsed;

        private IEnumerator<string> filePaths;

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

                using (var fs = File.OpenRead(filePaths.Current))
                {
                    ImageResult image = ImageResult.FromStream(fs);
                    atlas.AddImage(PixelFormat.Rgba, PixelType.Byte, image.Data.AsSpan());
                }

                previousElapsed = timer.Elapsed - start;
            } while (timer.Elapsed + previousElapsed < maxTimeHint && filePaths.MoveNext());
        }

    }
}
