using DMinecraft.PhysicalClient.Scheduling.Coroutines;
using FreeTypeBinding;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Text.Content
{
    internal unsafe class FontCreationCoroutine : ITimedCoroutine
    {
        public bool IsCompleted {get; private set;}

        public int FontSizePixels { get; }

        private readonly FT_FaceRec_* ftFace;

        private IEnumerator<GlyphRange> glyphRanges;

        private Stopwatch stopwatch;

        private TimeSpan previousElapsed;

        private GlyphRange? currentRange;

        private uint currentGlyphIndex;

        private TimeSpan maxTimeHint;

        private FT_Render_Mode_ ftRenderMode;

        private uint lastGlyphIndex;

        //private int[]

        public FontCreationCoroutine(FT_FaceRec_* ftFace, IEnumerator<GlyphRange> glyphRanges, int sizePixels, bool isSdf)
        {
            this.ftFace = ftFace;
            this.glyphRanges = glyphRanges;
            FontSizePixels = sizePixels;
            previousElapsed = TimeSpan.Zero;
            stopwatch = new Stopwatch();
            ftRenderMode = isSdf ? FT_Render_Mode_.FT_RENDER_MODE_SDF : FT_Render_Mode_.FT_RENDER_MODE_NORMAL;
            
        }

        public void Step(TimeSpan maxTimeHint)
        {
            if(currentRange == null)
            {
                if(glyphRanges.MoveNext())
                {
                    if (lastGlyphIndex > glyphRanges.Current.Start)
                        throw new FontException("Invalid ranges");
                    currentRange = glyphRanges.Current;
                    currentGlyphIndex = currentRange.Start;
                }
                else
                {
                    IsCompleted = true;
                    return;
                }
            }

            for (uint i = currentGlyphIndex; i < currentRange.Count; i++)
            {
                FT.FT_Load_Glyph(ftFace, i, FT_LOAD.FT_LOAD_RENDER);
                FT.FT_Render_Glyph(ftFace->glyph, ftRenderMode);
            }

            lastGlyphIndex = currentRange.End;
            currentRange = null;
        }
    }
}
