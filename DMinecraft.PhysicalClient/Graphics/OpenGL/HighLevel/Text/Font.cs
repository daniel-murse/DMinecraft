using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Text.Content;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Textures.Atlas;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Text
{
    //for cursor positioning in ligatures, there is a table in otf, lig caret
    //kinda hard to access though through here. hb does have a method for it but
    //its not in the .net binding and we would need a bit more work than just lookinmg it up
    //so, for development speed, we ought to just place the caret at the end of the glyph position
    //or divide the width as a hack to place the caret
    internal class Font : IDisposable
    {
        private const uint c2e6 = 0b00111111u;
        private bool disposedValue;

        public GlyphAtlas GlyphAtlas { get; }

        public HarfBuzzSharp.Font HBFont { get; }

        public Vector2 HBFracScale { get; }

        public Font(string path)
        {
            //due to how hb works (reference counted objects) we should dispose these
            //after we create the stuff with them
            using (var blob = HarfBuzzSharp.Blob.FromFile(path))
            {
                using (var face = new HarfBuzzSharp.Face(blob, 0))
                {
                    HBFont = new HarfBuzzSharp.Font(face);
                }
            }
        }

        public Font(FTGlyphRange[] glyphs, HarfBuzzSharp.Font font)
        {
            GlyphAtlas = new GlyphAtlas(glyphs);
            HBFont = font;
        }

        public void SubmitText(HarfBuzzSharp.Buffer buffer, SpriteBatch spriteBatch)
        {
            var glyphs = buffer.GetGlyphInfoSpan();
            var positions = buffer.GetGlyphPositionSpan();
            
            int glyphIndex = 0;
            while (glyphIndex < glyphs.Length)
            {
                if(spriteBatch.IsFull)
                    spriteBatch.Flush();
                var sprites = spriteBatch.SubmitSprites(Math.Min(glyphs.Length - glyphIndex, spriteBatch.Remaining));

                for (global::System.Int32 i = 0; i < sprites.Length; i++)
                {
                    Glyph glyph = GlyphAtlas.GetGlyphByIndex(glyphs[glyphIndex].Codepoint);

                    Vector3 position = new Vector3(
                        positions[glyphIndex].XOffset / HBFracScale.X + glyph.PixelHoriBearingX2e6 / c2e6,
                        positions[glyphIndex].YOffset / HBFracScale.Y + glyph.PixelHoriBearingY2e6 / c2e6,
                        0);

                    sprites[i].BottomLeft.Position = position;

                    glyphIndex++;
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Font()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
