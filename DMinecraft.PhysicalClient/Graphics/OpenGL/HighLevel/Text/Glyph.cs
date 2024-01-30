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
        public required PackedTexture2DArrayAtlasItem Texture { get; init; }

        //26.6 fractional int (cast to float and divide by 2^6 to get the actual pixel value as a decimal)
        public required int PixelWidth2e6 { get; init; }
        public required int PixelHeight2e6 { get; init; }
        public required int PixelHoriBearingX2e6 { get; init; }
        public required int PixelHoriBearingY2e6 { get; init; }
        public required int PixelVertBearingX2e6 { get; init; }
        public required int PixelVertBearingY2e6 { get; init; }

    }
}
