using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Lines;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites.Packed
{
    internal class SpriteBatch
    {
        public SpriteMesh Mesh { get; }

        public SpriteBatch(GLContext glContext, int capacity, SpriteVertexDeclaration vertexDeclaration, string? name = null)
        {
            Mesh = new SpriteMesh(glContext, capacity, vertexDeclaration, name);
        }

        public int Capacity => Mesh.Sprites.Length;

        public int SpriteCount { get; private set; }

        public int Remaining => Mesh.Sprites.Length - SpriteCount;

        public bool IsFull => Remaining == 0;

        public void Clear()
        {
            SpriteCount = 0;
        }

        public void SubmitLines(Span<SpriteVertices> sprites)
        {
            if (sprites.Length > Remaining)
            {
                throw new GLGraphicsException("Sb capacity exceeded.");
            }
            sprites.CopyTo(Mesh.Sprites.AsSpan(SpriteCount));
            SpriteCount += sprites.Length;
        }

        public Span<SpriteVertices> SubmitSprites(int count)
        {
            if (count > Remaining)
                throw new GLGraphicsException("Sb capacity exceeded.");

            var span = Mesh.Sprites.AsSpan(SpriteCount, count);
            SpriteCount += count;
            return span;
        }

        public void Draw()
        {
            Mesh.Vbo.SubData(0, SpriteCount * SpriteVertices.SizeBytes, Mesh.Sprites.AsSpan());
            Mesh.Vao.Use();
            GL.DrawElements(PrimitiveType.Triangles, SpriteCount * 6, DrawElementsType.UnsignedShort, 0);
        }

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }
                Mesh?.Dispose();
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~LineBatch()
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
