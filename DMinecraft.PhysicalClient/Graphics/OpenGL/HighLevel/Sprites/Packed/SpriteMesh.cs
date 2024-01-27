using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects;
using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects.Data.VertexArrays;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Lines;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites.Packed
{
    internal class SpriteMesh : IDisposable
    {
        public GLVertexArray Vao { get; }

        public GLBuffer Vbo { get; }

        public GLBuffer Ebo { get; }

        public SpriteVertices[] Sprites { get; }
        public SpriteVertexDeclaration VertexDeclaration { get; }

        public SpriteMesh(GLContext glContext, int capacity, SpriteVertexDeclaration vertexDeclaration, string? name = null)
        {
            try
            {
                Sprites = new SpriteVertices[capacity];
                Vbo = new GLBuffer(glContext, name == null ? name : name + "_vbo");
                Vbo.CreateImmutable(capacity * SpriteVertices.SizeBytes, BufferStorageFlags.DynamicStorageBit, nint.Zero);
                Ebo = new GLBuffer(glContext, name == null ? name : name + "_ebo");
                Vao = new GLVertexArray(glContext, name);
                VertexDeclaration = vertexDeclaration;
                ConfigureVao();
                InitEbo();
            }
            catch (Exception)
            {
                Dispose();
                throw;
            }
        }

        private void InitEbo()
        {
            ushort[] indices = new ushort[Sprites.Length * 6];

            for (int i = 0; i < Sprites.Length; i++)
            {

                //bl tl br tr
                int spriteIndex = i * 6;
                indices[spriteIndex]     = (ushort)i;//bl
                indices[spriteIndex + 1] = (ushort)(i + 1);//tl
                indices[spriteIndex + 2] = (ushort)(i + 2);//br
                indices[spriteIndex + 3] = (ushort)(i + 1);//tl
                indices[spriteIndex + 4] = (ushort)(i + 3); //tr
                indices[spriteIndex + 5] = (ushort)(i + 2);//br
            }

            Ebo.CreateImmutable(sizeof(ushort) * indices.Length, BufferStorageFlags.None, indices.AsSpan());
            //Ebo.SubData(0, sizeof(ushort) * indices.Length, indices.AsSpan());
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

                Ebo?.Dispose();
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

        protected void ConfigureVao()
        {
            Vao.SetVertexBufferBinding(0, new VertexBufferBinding(SpriteVertex.SizeBytes, 0, Vbo));
            Vao.SetVertexAttributeFormat(VertexDeclaration.PositionLocation, new VertexAttributeFormat(3, VertexAttribType.Float, false, 0, false));
            Vao.SetVertexAttributeFormat(VertexDeclaration.ColorLocation, new VertexAttributeFormat(4, VertexAttribType.UnsignedByte, true, 12, false));
            Vao.SetVertexAttributeFormat(VertexDeclaration.UVLocation, new VertexAttributeFormat(2, VertexAttribType.UnsignedByte, true, 16, false));
            Vao.SetVertexAttributeFormat(VertexDeclaration.LayerLocation, new VertexAttributeFormat(1, VertexAttribType.Int, false, 18, true));
            Vao.SetVertexAttributeBinding(VertexDeclaration.PositionLocation, 0);
            Vao.SetVertexAttributeBinding(VertexDeclaration.ColorLocation, 0);
            Vao.SetVertexAttributeBinding(VertexDeclaration.UVLocation, 0);
            Vao.SetVertexAttributeBinding(VertexDeclaration.LayerLocation, 0);
            Vao.SetVertexAttributeEnabled(VertexDeclaration.PositionLocation, true);
            Vao.SetVertexAttributeEnabled(VertexDeclaration.ColorLocation, true);
            Vao.SetVertexAttributeEnabled(VertexDeclaration.UVLocation, true);
            Vao.SetVertexAttributeEnabled(VertexDeclaration.LayerLocation, true);
            Vao.IndexBuffer = Ebo;
        }
    }
}
