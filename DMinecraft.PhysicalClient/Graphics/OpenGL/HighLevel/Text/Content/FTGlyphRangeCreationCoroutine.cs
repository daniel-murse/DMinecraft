using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Textures.Atlas;
using DMinecraft.PhysicalClient.Scheduling.Coroutines;
using FreeTypeBinding;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Text.Content
{
    internal unsafe class FTGlyphRangeCreationCoroutine : ITimedCoroutine
    {
        [MemberNotNullWhen(true, nameof(GlyphRange))]
        public bool IsCompleted { get; private set; }

        public PackedTexture2DArrayAtlas Atlas { get; }

        private FT_FaceRec_* ftFace;
        private readonly uint glyphIndexStart;
        private readonly uint glyphIndexEnd;
        private Stopwatch stopwatch;

        private TimeSpan previousElapsed;

        private uint glyphIndex;

        private bool isSdf;

        private Glyph[] glyphs;

        public FTGlyphRange? GlyphRange { get; private set; }

        public FTGlyphRangeCreationCoroutine(FT_FaceRec_* ftFace, uint start, uint end, bool isSdf, PackedTexture2DArrayAtlas atlas)
        {
            this.ftFace = ftFace;
            this.glyphIndexStart = start;
            this.glyphIndexEnd = end;
            Atlas = atlas;
            this.isSdf = isSdf;
            this.stopwatch = new Stopwatch();
            this.previousElapsed = TimeSpan.Zero;
            glyphIndex = 0;
            glyphs = new Glyph[end - start];
        }

        public void Step(TimeSpan maxTimeHint)
        {
            if(glyphIndex + glyphIndexStart == glyphIndexEnd)
            {
                GlyphRange = new FTGlyphRange(glyphIndexStart, glyphIndexEnd, glyphs);
                IsCompleted = true;
                return;
            }

            stopwatch.Restart();

            do
            {
                TimeSpan start = stopwatch.Elapsed;

                FT_Error error;
                error = FT.FT_Load_Glyph(ftFace, glyphIndex + glyphIndexStart, FT_LOAD.FT_LOAD_DEFAULT);
                if (error != FT_Error.FT_Err_Ok)
                    throw new FontException();
                if(isSdf)
                {
                    error = FT.FT_Render_Glyph(ftFace->glyph, FT_Render_Mode_.FT_RENDER_MODE_NORMAL);
                    //could be err here but whatever the 2nd call will catch it probably
                    error = FT.FT_Render_Glyph(ftFace->glyph, FT_Render_Mode_.FT_RENDER_MODE_SDF);
                }
                else
                {
                    error = FT.FT_Render_Glyph(ftFace->glyph, FT_Render_Mode_.FT_RENDER_MODE_NORMAL);
                }
                if (error != FT_Error.FT_Err_Ok)
                {
                    //throw new FontException();
                    glyphIndex++;
                    continue;
                    //todo maybe report not finding this
                }
                int length = (int)(ftFace->glyph->bitmap.rows * ftFace->glyph->bitmap.width);
                var atlasItem = Atlas.AddImage(PixelFormat.Red, PixelType.UnsignedByte, (int)ftFace->glyph->bitmap.rows, (int)ftFace->glyph->bitmap.width, new Span<byte>(ftFace->glyph->bitmap.buffer, length));

                int pixelHeight2e6 = ftFace->glyph->metrics.height;
                int pixelWidth2e6 = ftFace->glyph->metrics.width;
                int horiBearingX2e6 = ftFace->glyph->metrics.horiBearingX;
                int horiBearingY2e6 = ftFace->glyph->metrics.horiBearingY;
                int vertBearingX2e6 = ftFace->glyph->metrics.vertBearingX;
                int vertBearingY2e6 = ftFace->glyph->metrics.vertBearingY;

                var glyph = new Glyph() { Texture = atlasItem, PixelWidth2e6 = pixelWidth2e6, PixelHeight2e6 = pixelHeight2e6, PixelHoriBearingX2e6 = horiBearingX2e6, PixelHoriBearingY2e6 = horiBearingY2e6, PixelVertBearingX2e6 = vertBearingX2e6, PixelVertBearingY2e6 = vertBearingY2e6 };

                glyphs[glyphIndex] = glyph;

                glyphIndex++;
                previousElapsed = stopwatch.Elapsed - start;
            } while (stopwatch.Elapsed + previousElapsed < maxTimeHint && glyphIndex + glyphIndexStart < glyphIndexEnd);
        }
    }
}
