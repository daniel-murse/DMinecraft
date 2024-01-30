using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Text.Content;
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

        public GlyphAtlas(FTGlyphRange[] glyphRanges)
        {
            this.glyphRanges = glyphRanges;
        }

        public Glyph GetGlyphByIndex(uint index)
        {
            var range = glyphRanges.Where(p => p.Start < index && p.End < index).FirstOrDefault();
            if (range == null)
                throw new FontException("Range not found.");
            return range.Glyphs[index] ?? throw new FontException("Glyph not found.");
        }
    }
}
