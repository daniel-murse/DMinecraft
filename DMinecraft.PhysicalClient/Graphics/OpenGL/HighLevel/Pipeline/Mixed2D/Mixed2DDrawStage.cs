using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Lines;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites.Batches;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites.Optim;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites.Renderers;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Pipeline.Mixed2D
{
    internal class Mixed2DDrawStage : IRenderPipelineStage
    {
        [SetsRequiredMembers]
        public Mixed2DDrawStage(GLContext gLContext, LineRenderBatch defaultLineRenderBatch, SpriteRenderBatch defaultSpriteRenderBatch)
        {
            LineRenderBatches = new List<LineRenderBatch>();
            SpriteRenderBatches = new List<SpriteRenderBatch>();
            DefaultLineRenderBatch = defaultLineRenderBatch;
            DefaultSpriteRenderBatch = defaultSpriteRenderBatch;
            GLContext = gLContext;
        }

        public LineRenderBatch DefaultLineRenderBatch { get; set; }

        public SpriteRenderBatch DefaultSpriteRenderBatch { get; set; }

        public IList<LineRenderBatch> LineRenderBatches { get; }

        public IList<SpriteRenderBatch> SpriteRenderBatches { get; }

        public required GLContext GLContext { get; set; }

        public event Action<Mixed2DDrawStage> OnDraw;

        public void Execute()
        {
            Clear();
            OnDraw?.Invoke(this);
            Flush();
        }

        private void Flush()
        {
            if (LineRenderBatches != null)
                foreach (var batch in LineRenderBatches)
                {
                    batch.Flush();
                }
            DefaultLineRenderBatch?.Flush();
            if (SpriteRenderBatches != null)
                foreach (var batch in SpriteRenderBatches)
                {
                    batch.Flush();
                }
            DefaultSpriteRenderBatch?.Flush();
        }

        private void Clear()
        {
            if (LineRenderBatches != null)
                foreach (var batch in LineRenderBatches)
                {
                    batch.LineBatch.Clear();
                }
            if (SpriteRenderBatches != null)
                foreach (var batch in SpriteRenderBatches)
                {
                    batch.SpriteBatch.Clear();
                }
            DefaultLineRenderBatch?.LineBatch.Clear();
            DefaultSpriteRenderBatch?.SpriteBatch.Clear();
        }
    }
}
