using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites
{
    internal class SpriteDrawStage : IRenderPipelineStage
    {
        public SpriteBatch SpriteBatch { get; }

        public event Action<SpriteBatch> OnDraw;

        public void Execute()
        {
            SpriteBatch.Clear();
            OnDraw?.Invoke(SpriteBatch);
            SpriteBatch.Draw();
        }
    }
}
