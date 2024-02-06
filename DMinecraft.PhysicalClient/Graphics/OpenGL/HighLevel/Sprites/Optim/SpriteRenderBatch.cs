using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites.Batches;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites.Renderers;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites.Vertices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites.Optim
{
    //todo maybe remove the interface and use this directly
    internal class SpriteRenderBatch : ISpriteVerticesBatchConsumer
    {
        public SpriteRenderBatch(SpriteBatch spriteBatch, SpriteRenderer spriteRenderer, int albedoUnit)
        {
            SpriteBatch = spriteBatch;
            SpriteRenderer = spriteRenderer;
            AlbedoUnit = albedoUnit;
        }

        public SpriteBatch SpriteBatch { get; set; }

        public SpriteRenderer SpriteRenderer { get; set; }
        public int AlbedoUnit { get; set; }
        public GLTexture Texture { get; set; }

        public int Remaining => ((ISpriteVerticesBatchConsumer)SpriteBatch).Remaining;

        public bool IsFull => ((ISpriteVerticesBatchConsumer)SpriteBatch).IsFull;

        public GLContext Context => SpriteRenderer.Program.Context;

        public void UseTexture(GLTexture texture)
        {
            if(Texture != texture)
            {
                Flush();
            }
            Texture = texture;
        }

        public Span<SpriteVertices> SubmitSpritesAF(int count)
        {
            if (SpriteBatch.Remaining < count)
            {
                Flush();
            }
            return SpriteBatch.SubmitSprites(count);
        }

        public void Flush()
        {
            SpriteRenderer.UseProgram();
            Context.BindTexture2DArray(Texture, AlbedoUnit);
            SpriteBatch.Flush();
        }

        public Span<SpriteVertices> SubmitSprites(int spriteCount)
        {
            return ((ISpriteVerticesBatchConsumer)SpriteBatch).SubmitSprites(spriteCount);
        }
    }
}
