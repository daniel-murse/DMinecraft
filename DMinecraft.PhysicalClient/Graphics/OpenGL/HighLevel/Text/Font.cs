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

        public void SubmitText(HarfBuzzSharp.Buffer buffer, SpriteBatch spriteBatch, Color4 color)
        {
            var glyphs = buffer.GetGlyphInfoSpan();
            var positions = buffer.GetGlyphPositionSpan();

            Vector3 cursor = Vector3.Zero;

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

                    const int i2e6 = 0b01000000;

                    Vector3 position = new Vector3(
                        positions[glyphIndex].XOffset / HBFracScale.X + glyph.PixelHoriBearingX2e6 / i2e6,
                        positions[glyphIndex].YOffset / HBFracScale.Y + (-glyph.PixelHeight2e6 + glyph.PixelHoriBearingY2e6) / i2e6,
                        0);
                    position += cursor;

                    sprites[i].BottomLeft.Position = position;
                    sprites[i].BottomRight.Position = position;
                    sprites[i].BottomRight.Position.X += glyph.PixelWidth2e6 / (float)i2e6;
                    sprites[i].TopRight.Position = position;
                    sprites[i].TopRight.Position.X += glyph.PixelWidth2e6 / (float)i2e6;
                    sprites[i].TopRight.Position.Y += glyph.PixelHeight2e6 / (float)i2e6;
                    sprites[i].TopLeft.Position = position;
                    sprites[i].TopLeft.Position.Y += glyph.PixelHeight2e6 / (float)i2e6;

                    sprites[i].BottomLeft.Index = glyph.Texture.Layer;
                    sprites[i].BottomRight.Index = glyph.Texture.Layer;
                    sprites[i].TopLeft.Index = glyph.Texture.Layer;
                    sprites[i].TopRight.Index = glyph.Texture.Layer;

                    sprites[i].BottomLeft.Color = color.ToRgba();
                    sprites[i].BottomRight.Color = color.ToRgba();
                    sprites[i].TopLeft.Color = color.ToRgba();
                    sprites[i].TopRight.Color = color.ToRgba();

                    sprites[i].BottomLeft.U = glyph.Texture.Left;
                    sprites[i].BottomLeft.V = glyph.Texture.Bottom;
                    sprites[i].BottomRight.U = glyph.Texture.Right;
                    sprites[i].BottomRight.V = glyph.Texture.Bottom;
                    sprites[i].TopLeft.U = glyph.Texture.Left;
                    sprites[i].TopLeft.V = glyph.Texture.Top;
                    sprites[i].TopRight.U = glyph.Texture.Right;
                    sprites[i].TopRight.V = glyph.Texture.Top;

                    sprites[i].BottomLeft.Layer = glyph.Texture.Layer;
                    sprites[i].TopLeft.Layer = glyph.Texture.Layer;
                    sprites[i].BottomRight.Layer = glyph.Texture.Layer;
                    sprites[i].TopRight.Layer = glyph.Texture.Layer;

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
