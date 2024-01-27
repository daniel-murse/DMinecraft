using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Textures.Atlas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Text
{
    internal class Font
    {
        public Texture2DArrayAtlas GlyphAtlas { get; }

        public HarfBuzzSharp.Font HarfBuzzFont { get; }

        public Font(string fontFilePath)
        {
            //due to how hb works (reference counted objects) we should dispose these
            //after we create the stuff with them
            using (var blob = HarfBuzzSharp.Blob.FromFile(fontFilePath))
            {
                using (var face = new HarfBuzzSharp.Face(blob, 0))
                {
                    HarfBuzzFont = new HarfBuzzSharp.Font(face);
                }
            }
        }

        public void SubmitText(HarfBuzzSharp.Buffer buffer)
        {
            var glyphs = buffer.GetGlyphInfoSpan();
            var positions = buffer.GetGlyphPositionSpan();

            for (int i = 0; i < glyphs.Length; i++)
            {
                //the font glyph id/index now
                //glyphs[i].Codepoint
                
            }
        }
    }
}
