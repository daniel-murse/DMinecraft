using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Text.Content
{
    internal class GlyphRange
    {
        public GlyphRange()
        {
        }

        public GlyphRange(uint begin, uint end)
        {
            Start = begin;
            Count = end;
        }

        public uint Start { get; init; }

        public uint Count { get; init; }  
    
        public uint End => Start + Count;
    }
}
