using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Textures
{
    internal class TextureRegion
    {
        public required UVu BottomLeft;

        public required UVu TopLeft;

        public required UVu TopRight;

        public required UVu BottomRight;

        [SetsRequiredMembers]
        public TextureRegion(UVu bottomLeft, UVu topLeft, UVu topRight, UVu bottomRight, int layer, int index, GLTexture texture)
        {
            BottomLeft = bottomLeft;
            TopLeft = topLeft;
            TopRight = topRight;
            BottomRight = bottomRight;
            Layer = layer;
            Index = index;
            Texture = texture;
        }

        public required int Layer { get; init; }

        public required int Index { get; init; }

        public required GLTexture Texture { get; init; }
    }
}
