using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Text.Content
{
    internal class FTGlyphRange
    {
        public FTGlyphRange(uint begin, uint end, Glyph[] glyphs)
        {
            Start = begin;
            End = end;
            Glyphs = glyphs;
        }

        public uint Start { get; }

        public uint End { get; }  
    
        public uint Count => End - Start;

        public Glyph[] Glyphs { get; }
    }
}
