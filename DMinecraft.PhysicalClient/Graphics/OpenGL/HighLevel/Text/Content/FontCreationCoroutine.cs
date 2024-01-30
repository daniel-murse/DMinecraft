using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Textures.Atlas;
using DMinecraft.PhysicalClient.Scheduling.Coroutines;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Text.Content
{
    internal unsafe class FontCreationCoroutine : ITimedCoroutine, IDisposable
    {
        public Font? Font { get; private set; }

        private PackedTexture2DArrayAtlas atlas;

        private FTFontCreationCoroutine coroutine;

        public FontCreationCoroutine(FreeTypeBinding.FT_FaceRec_* ftFace, HarfBuzzSharp.Face hbFace, IEnumerable<(uint start, uint end)> glyphRanges, int sizePixels, bool isSdf, GLContext glContext)
        {
            var count = glyphRanges.Sum(p => p.Item2 - p.Item1);
            var sqrtCount = (int)Math.Ceiling(Math.Sqrt(count));
            //todo handle case div = 0
            var rowCol = (int)Math.Ceiling(glContext.MaxTextureDimensionSize / (float)(sqrtCount * sizePixels));
            var depth = (int)Math.Ceiling((float)count / (rowCol * rowCol));


            atlas = new PackedTexture2DArrayAtlas(glContext, new PackedTexture2DArrayAtlasOptions() { MipLevels = 1, ImageWidth = sizePixels, ImageHeight = sizePixels, Rows = rowCol, Cols = rowCol, Depth = depth, InternalFormat = InternalFormat.R8});
            atlas.Texture.Swizzle = new All[] { All.One, All.One, All.One, All.Red };
            atlas.Texture.MinFilter = TextureMinFilter.Linear;
            atlas.Texture.MagFilter = TextureMagFilter.Linear;
            coroutine = new FTFontCreationCoroutine(ftFace, glyphRanges, sizePixels, isSdf, atlas);
            HbFace = hbFace;
            SizePixels = sizePixels;
        }

        [MemberNotNullWhen(true, nameof(Font))]
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
                //only end up diving by frav part to  get puixels in hb font
                Font = new Font(atlas, coroutine.GlyphRanges, hbFont, new OpenTK.Mathematics.Vector2(i2e6, i2e6));
            }
        }

        public void Dispose()
        {
            if(!IsCompleted)
                ((IDisposable)atlas).Dispose();
        }
    }
}
