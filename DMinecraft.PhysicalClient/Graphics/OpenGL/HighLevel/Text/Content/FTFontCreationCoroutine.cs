using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Textures.Atlas;
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
    internal unsafe class FTFontCreationCoroutine : ITimedCoroutine
    {
        public bool IsCompleted { get; private set; }

        public int FontSizePixels { get; }

        private readonly FT_FaceRec_* ftFace;

        private IEnumerator<Tuple<uint, uint>> glyphRanges;

        private List<FTGlyphRange> glyphRangesList;

        private bool isSdf;

        private SequentialCoroutinesCoroutine coroutine;

        private FTGlyphRangeCreationCoroutine[] coroutines;

        public FTGlyphRange[] GlyphRanges { get; private set; }
            
        public FTFontCreationCoroutine(FT_FaceRec_* ftFace, IEnumerable<(uint start, uint end)> glyphRanges, int sizePixels, bool isSdf, PackedTexture2DArrayAtlas atlas)
        {
            this.ftFace = ftFace;
            FontSizePixels = sizePixels;
            this.isSdf = isSdf;
            coroutines = glyphRanges.Select(p => new FTGlyphRangeCreationCoroutine(ftFace, p.start, p.end, isSdf, atlas)).ToArray();
            coroutine = new SequentialCoroutinesCoroutine(coroutines.AsEnumerable().GetEnumerator());

            if (FT_Error.FT_Err_Ok != FT.FT_Set_Pixel_Sizes(ftFace, (uint)sizePixels, (uint)sizePixels))
                throw new FontException();
        }

        public void Step(TimeSpan maxTimeHint)
        {
            coroutine.Step(maxTimeHint);
            IsCompleted = coroutine.IsCompleted;
            if (IsCompleted)
            {
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
                GlyphRanges = coroutines.Select(p => p.GlyphRange).ToArray();
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
            }
        }
    }
}