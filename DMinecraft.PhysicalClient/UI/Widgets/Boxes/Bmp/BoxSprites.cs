using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites.Optim;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Textures;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.UI.Widgets.Boxes.Bmp
{
    internal class BoxSprites
    {
        public TextureRegion Fill { get; }
        public TextureRegion Edge { get; }
        public TextureRegion Corner { get; }
        public int DimensionSizePixels { get; }
        public Vector2 SizePixels { get; }
        public GLTexture Texture { get; }
        public SpriteRenderBatch SpriteRenderBatch { get; }
    }
}
