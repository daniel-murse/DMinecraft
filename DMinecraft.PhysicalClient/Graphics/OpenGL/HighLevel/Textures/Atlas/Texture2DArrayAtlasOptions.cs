using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Textures.Atlas
{
    internal class Texture2DArrayAtlasOptions
    {
        public Texture2DArrayAtlasOptions() { InternalFormat = InternalFormat.Rgba8; MipLevels = 0; }

        public required int Width { get; init; }
        public required int Height { get; init; }
        public required int Depth { get; init; }
        public int MipLevels { get; init; }
        public InternalFormat InternalFormat { get; init; }
    }
}
