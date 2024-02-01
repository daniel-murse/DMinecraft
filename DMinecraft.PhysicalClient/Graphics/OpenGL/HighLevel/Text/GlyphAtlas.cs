using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Text.Content;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Textures.Atlas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Text
{
    internal class GlyphAtlas
    {
        private FTGlyphRange[] glyphRanges;

        public GlyphAtlas(FTGlyphRange[] glyphRanges, PackedTexture2DArrayAtlas atlas)
        {
            this.glyphRanges = glyphRanges;
            Atlas = atlas;
        }

        public PackedTexture2DArrayAtlas Atlas { get; }

        public Glyph GetGlyphByIndex(uint index)
        {
            var range = glyphRanges.Where(p => p.Start <= index && p.End > index).FirstOrDefault();
            if (range == null)
                throw new FontException("Range not found.");
            return range?.Glyphs[index - range.Start] ?? throw new FontException("Glyph not found.");
        }

        public Glyph? TryGetGlyphByIndex(uint index)
        {
            var range = glyphRanges.Where(p => p.Start <= index && p.End > index).FirstOrDefault();
            return range?.Glyphs[index - range.Start] ?? null;
        }
    }
}
