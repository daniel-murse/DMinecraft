using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites.Optim
{
    internal class SpriteRenderResources
    {
        public SpriteBatch SpriteBatch { get; set; }
        public SpriteRenderer Renderer { get; set; }
        public GLTexture Texture { get; set; }
    }
}
