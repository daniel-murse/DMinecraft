using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Textures.Atlas;
using DMinecraft.PhysicalClient.Scheduling.Coroutines;
using FreeTypeBinding;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Text.Content
{
    internal unsafe class GlyphRangeCreationCoroutine : ITimedCoroutine
    {
        public bool IsCompleted { get; private set; }

        public int FontSizePixels { get; }
        public PackedTexture2DArrayAtlas Atlas { get; }

        private FT_FaceRec_* ftFace;

        private Stopwatch stopwatch;

        private TimeSpan previousElapsed;

        private GlyphRange range;

        private uint glyphIndex;

        private FT_Render_Mode_ ftRenderMode;

        public GlyphRangeCreationCoroutine(FT_FaceRec_* ftFace, int fontSizePixels, GlyphRange range, bool isSdf, PackedTexture2DArrayAtlas atlas)
        {
            this.ftFace = ftFace;
            FontSizePixels = fontSizePixels;
            this.range = range;
            Atlas = atlas;
            this.ftRenderMode = isSdf ? FT_Render_Mode_.FT_RENDER_MODE_SDF : FT_Render_Mode_.FT_RENDER_MODE_NORMAL;
            this.stopwatch = new Stopwatch();
            this.previousElapsed = TimeSpan.Zero;
            glyphIndex = range.Start;
        }

        public void Step(TimeSpan maxTimeHint)
        {
            if(glyphIndex == range.End)
            {
                IsCompleted = true;
                return;
            }

            do
            {
                FT_Error error;
                error = FT.FT_Load_Glyph(ftFace, glyphIndex, FT_LOAD.FT_LOAD_DEFAULT);
                if (error != FT_Error.FT_Err_Ok)
                    throw new FontException();
                error = FT.FT_Render_Glyph(ftFace->glyph, ftRenderMode);
                if (error != FT_Error.FT_Err_Ok)
                    throw new FontException();
                int length = (int)(ftFace->glyph->bitmap.rows * ftFace->glyph->bitmap.width);
                Atlas.AddImage(PixelFormat.Red, PixelType.UnsignedByte, (int)ftFace->glyph->bitmap.rows, (int)ftFace->glyph->bitmap.width, new Span<byte>(ftFace->glyph->bitmap.buffer, length));

                glyphIndex++;
                previousElapsed = stopwatch.Elapsed;
            } while (stopwatch.Elapsed + previousElapsed < maxTimeHint && glyphIndex < range.End);
        }
    }
}
