using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects;
using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects.Data.VertexArrays;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Lines
{
    internal class LineMesh : IDisposable
    {
        public GLVertexArray Vao { get; }

        public GLBuffer Vbo { get; }

        public LineVertices[] Lines { get; }
        public LineVertexDeclaration VertexDeclaration { get; }

        public LineMesh(GLContext glContext, int capacity, LineVertexDeclaration vertexDeclaration, string? name = null)
        {
            try
            {
                Vbo = new GLBuffer(glContext, name);
                Vbo.CreateImmutable(capacity * LineVertices.SizeBytes, BufferStorageFlags.DynamicStorageBit, nint.Zero);
                Vao = new GLVertexArray(glContext, name);
                Vao.SetVertexBufferBinding(0, new VertexBufferBinding(LineVertex.SizeBytes, 0, Vbo));
                Vao.SetVertexAttributeFormat(vertexDeclaration.PositionLocation, new VertexAttributeFormat(3, VertexAttribType.Float, false, 0, false));
                Vao.SetVertexAttributeFormat(vertexDeclaration.ColorLocation, new VertexAttributeFormat(4, VertexAttribType.Float, false, 12, false));
                Vao.SetVertexAttributeBinding(vertexDeclaration.PositionLocation, 0);
                Vao.SetVertexAttributeBinding(vertexDeclaration.ColorLocation, 0);
                Vao.SetVertexAttributeEnabled(vertexDeclaration.PositionLocation, true);
                Vao.SetVertexAttributeEnabled(vertexDeclaration.ColorLocation, true);
                Lines = new LineVertices[capacity];
            }
            catch (Exception)
            {
                Dispose();
                throw;
            }
            VertexDeclaration = vertexDeclaration;
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

                Vao?.Dispose();
                Vbo?.Dispose();

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~LineBatchMesh()
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
