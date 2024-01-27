using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Textures.Atlas
{
    internal class PackedTexture2DArrayAtlasItem
    {
        public PackedTexture2DArrayAtlasItem()
        {
        }

        [SetsRequiredMembers]
        public PackedTexture2DArrayAtlasItem(PackedTexture2DArrayAtlas atlas, byte left, byte bottom, byte width, byte height, int layer)
        {
            Atlas = atlas;
            Left = left;
            Bottom = bottom;
            Right = width;
            Top = height;
            Layer = layer;
        }

        public required PackedTexture2DArrayAtlas Atlas { get; init; }

        public required byte Left { get; init; }

        public required byte Bottom { get; init; }

        public required byte Right { get; init; }

        public required byte Top { get; init; }
        
        public required int Layer { get; init; }
    }
}
