using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Textures.Atlas
{
    internal class PackedTexture2DArrayAtlasOptions
    {
        public required int ImageWidth { get; init; }
        public required int ImageHeight { get; init; }
        public required int Rows { get; init; }
        public required int Cols { get; init; }
        public required int Depth { get; init; }
        public int MipLevels { get; init; } = 1;
        public InternalFormat InternalFormat { get; init; } = InternalFormat.Red;

        public int TextureWidth => ImageWidth * Rows;
        public int TextureHeight => ImageHeight * Cols;
    }
}
