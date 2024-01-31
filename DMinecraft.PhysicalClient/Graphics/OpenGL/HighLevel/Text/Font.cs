using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Text.Content;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Textures.Atlas;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Util;
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
        private PackedTexture2DArrayAtlas atlas;

        public GlyphAtlas GlyphAtlas { get; }

        public HarfBuzzSharp.Font HbFont { get; }

        public Vector2 HBFracScale { get; }

        public Font(string path)
        {
            //due to how hb works (reference counted objects) we should dispose these
            //after we create the stuff with them
            using (var blob = HarfBuzzSharp.Blob.FromFile(path))
            {
                using (var face = new HarfBuzzSharp.Face(blob, 0))
                {
                    HbFont = new HarfBuzzSharp.Font(face);
                }
            }
        }

        public Font(PackedTexture2DArrayAtlas atlas, FTGlyphRange[] glyphs, HarfBuzzSharp.Font font, Vector2 hbFracScale)
        {
            HBFracScale = hbFracScale;
            GlyphAtlas = new GlyphAtlas(glyphs);
            HbFont = font;
            this.atlas = atlas;
        }

        public void SubmitText(HarfBuzzSharp.Buffer buffer, SpriteBatch spriteBatch, Transform transform, Color4 color)
        {
            var glyphs = buffer.GetGlyphInfoSpan();
            var positions = buffer.GetGlyphPositionSpan();

            Vector3 cursor = Vector3.Zero;
            //we want glyphs to keep their origin relative to the whole text dimensions
            //not their own axis and size (glyphs shouldnt't be individually rotated and scaled)
            //(thats not what we want unless we want a graphic effect which isnt UI friendly for other purposes)

            //Matrix4 matrix = transform.Matrix;

            int glyphIndex = 0;
            while (glyphIndex < glyphs.Length)
            {
                if(spriteBatch.IsFull)
                    spriteBatch.Flush();
                var sprites = spriteBatch.SubmitSprites(Math.Min(glyphs.Length - glyphIndex, spriteBatch.Remaining));


                for (global::System.Int32 i = 0; i < sprites.Length; i++)
                {
                    Glyph? glyph = GlyphAtlas.TryGetGlyphByIndex(glyphs[glyphIndex].Codepoint);


                    if (glyph == null)
                    {
                        cursor += new Vector3(positions[i].XAdvance, positions[i].YAdvance, 0) / new Vector3(HBFracScale.X, HBFracScale.Y, 1);
                        glyphIndex++;
                        continue;
                    }

                    //26.6 precision conversion factor
                    const int i2e6 = 0b01000000;

                    Vector3 position = new Vector3(
                        positions[glyphIndex].XOffset / HBFracScale.X + glyph.PixelHoriBearingX2e6 / i2e6,
                        positions[glyphIndex].YOffset / HBFracScale.Y + (-glyph.PixelHeight2e6 + glyph.PixelHoriBearingY2e6) / i2e6,
                        0);

                    //origin -= cursor;

                    position += cursor;

                    Vector2 size = new Vector2(glyph.PixelWidth2e6 / (float)i2e6, glyph.PixelHeight2e6 / (float)i2e6);

                    sprites[i].Compute(position, transform, size, glyph.Texture.Left, glyph.Texture.Bottom, glyph.Texture.Right, glyph.Texture.Top, glyph.Texture.Layer, 0, color);

                    cursor += new Vector3(positions[i].XAdvance, positions[i].YAdvance, 0) / new Vector3(HBFracScale.X, HBFracScale.Y, 1);
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
                this.atlas?.Dispose();
                this.HbFont.Dispose();
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
