using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Lines;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Pipeline.Mixed2D
{
    internal class Mixed2DDrawStage : IRenderPipelineStage
    {
        public required SpriteBatch SpriteBatch { get; set; }

        public required SpriteRenderer SpriteRenderer { get; set; }

        public required LineBatch LineBatch { get; set; }

        public required LineRenderer LineRenderer { get; set; }

        public required GLContext GLContext { get; set; }

        public event Action<Mixed2DDrawStage> OnDraw;

        public void Execute()
        {
            SpriteBatch.Clear();
            LineBatch.Clear();
            OnDraw?.Invoke(this);
            SpriteBatch.Draw();
            LineBatch.Draw();
        }
    }
}
