using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Pipeline;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites.Debug
{
    internal class DebugSpriteDrawStage : IRenderPipelineStage
    {
        [SetsRequiredMembers]
        public DebugSpriteDrawStage(SpriteBatch spriteBatch, SpriteRenderer spriteRenderer)
        {
            SpriteBatch = spriteBatch;
        }

        public required SpriteRenderer Renderer { get; init; }

        public required SpriteBatch SpriteBatch { get; init; }

        public GLContext GLContext => Renderer.Program.Context;

        public event Action<DebugSpriteDrawStage>? Draw;

        public void Execute()
        {
            SpriteBatch.Clear();
            Renderer.Program.Use();
            Draw?.Invoke(this);
            SpriteBatch.Draw();
        }
    }
}
