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
        public PackedTexture2DArrayAtlasItem(PackedTexture2DArrayAtlas atlas, ushort left, ushort bottom, ushort right, ushort top, int layer)
        {
            Atlas = atlas;
            Left = left;
            Bottom = bottom;
            Right = right;
            Top = top;
            Layer = layer;
        }

        public required PackedTexture2DArrayAtlas Atlas { get; init; }

        public required ushort Left { get; init; }

        public required ushort Bottom { get; init; }

        public required ushort Right { get; init; }

        public required ushort Top { get; init; }
        
        public required int Layer { get; init; }
    }
}
