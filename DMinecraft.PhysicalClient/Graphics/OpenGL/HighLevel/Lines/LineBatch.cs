using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Lines
{
    internal class LineBatch : IDisposable
    {
        public LineMesh Mesh { get; }

        public LineBatch(GLContext glContext, int capacity, LineVertexDeclaration vertexDeclaration, string? name = null)
        {
            Mesh = new LineMesh(glContext, capacity, vertexDeclaration, name);
        }

        public int Capacity => Mesh.Lines.Length;

        public int LineCount { get; private set; }

        public int Remaining => Mesh.Lines.Length - LineCount;

        public bool IsFull => Remaining == 0;

        public void Clear()
        {
            LineCount = 0;
        }

        public void SubmitLines(Span<LineVertices> lines)
        {
            if(lines.Length > Remaining)
            {
                throw new GLGraphicsException("Linebatch capacity exceeded.");
            }
            lines.CopyTo(Mesh.Lines.AsSpan(LineCount));
            LineCount += lines.Length;
        }

        public Span<LineVertices> SubmitLines(int count)
        {
            if(count > Remaining)
                throw new GLGraphicsException("Linebatch capacity exceeded.");

            var span = Mesh.Lines.AsSpan(LineCount, count);
            LineCount += count;
            return span;
        }

        public void Draw()
        {//todo name is fucked, optimisme
            Mesh.Vbo.SubData(0, LineCount * LineVertices.SizeBytes, Mesh.Lines.AsSpan());
            Mesh.Vao.Use();
            GL.DrawArrays(PrimitiveType.Lines, 0, LineCount * 2);
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

        internal void Flush()
        {
            Draw();
            Clear();
        }
    }
}
