using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites.Optim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Text
{
    internal class FontSpriteResources
    {
        public required Font Font { get; init; }

        public required SpriteRenderBatch RenderBatch { get; init; }
    }
}
