using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Textures.Atlas;
using DMinecraft.PhysicalClient.Scheduling.Coroutines;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Text.Content
{
    internal unsafe class FontCreationCoroutine : ITimedCoroutine
    {
        public PackedTexture2DArrayAtlas Atlas { get; }

        private FTFontCreationCoroutine coroutine;

        public FontCreationCoroutine(FreeTypeBinding.FT_FaceRec_* ftFace, HarfBuzzSharp.Face hbFace, IEnumerable<(uint start, uint end)> glyphRanges, int sizePixels, bool isSdf, GLContext glContext)
        {
            var count = glyphRanges.Sum(p => p.Item2 - p.Item1);
            var rowCol = glContext.MaxTextureDimensionSize / sizePixels;
            var depth = (int)Math.Ceiling((float)count / rowCol * rowCol);


            Atlas = new PackedTexture2DArrayAtlas(glContext, new PackedTexture2DArrayAtlasOptions() { MipLevels = 1, ImageWidth = sizePixels, ImageHeight = sizePixels, Rows = rowCol, Cols = rowCol, Depth = depth, InternalFormat = InternalFormat.Red});
            coroutine = new FTFontCreationCoroutine(ftFace, glyphRanges, sizePixels, isSdf, Atlas);
            HbFace = hbFace;
            SizePixels = sizePixels;
        }

        public bool IsCompleted { get; private set; }
        public HarfBuzzSharp.Face HbFace { get; }
        public int SizePixels { get; }

        public void Step(TimeSpan maxTimeHint)
        {
            coroutine.Step(maxTimeHint);

            if(!IsCompleted)
            {
                IsCompleted = coroutine.IsCompleted;
                var hbFont = new HarfBuzzSharp.Font(HbFace);
                const int i2e6 = 0b00100000;
                hbFont.SetScale(SizePixels * i2e6, SizePixels * i2e6);
                new Font(coroutine.GlyphRanges, hbFont);
            }
        }
    }
}
