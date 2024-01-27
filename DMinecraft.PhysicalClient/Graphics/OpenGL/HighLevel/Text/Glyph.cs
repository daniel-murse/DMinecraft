using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Textures.Atlas;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Text
{
    internal class Glyph
    {
        public Glyph(PackedTexture2DArrayAtlasItem texture, Vector2i horiBearing)
        {
            Texture = texture;
            HoriBearing = horiBearing;
        }

        public required PackedTexture2DArrayAtlasItem Texture { get; init; }

        public required Vector2i HoriBearing { get; init; }
    }
}
