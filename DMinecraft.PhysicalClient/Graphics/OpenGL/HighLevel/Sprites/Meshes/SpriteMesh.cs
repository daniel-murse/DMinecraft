using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects;
using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects.Data.VertexArrays;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Lines;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites.Vertices;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites.Meshes
{
    internal class SpriteMesh : IDisposable
    {
        public SpriteMeshVao SpriteVao { get; }

        private bool ownsSpriteVao;

        public GLBuffer Vbo { get; }

        public SpriteVertices[] Sprites { get; }

        public SpriteMesh(GLContext glContext, int capacity, SpriteVertexDeclaration vertexDeclaration, string? name = null)
        {
            ownsSpriteVao = true;
            try
            {
                Vbo = new GLBuffer(glContext, name == null ? name : name + "_vbo");
                Vbo.CreateImmutable(capacity * SpriteVertices.SizeBytes, BufferStorageFlags.DynamicStorageBit, nint.Zero);
                SpriteVao = new SpriteMeshVao(glContext, vertexDeclaration, capacity, name);
                Sprites = new SpriteVertices[capacity];
                SpriteVao.UseVbo(Vbo);
            }
            catch (Exception)
            {
                Dispose();
                throw;
            }
        }

        public SpriteMesh(GLContext glContext, int capacity, SpriteMeshVao spriteVao, string? name = null)
        {
            //explicit
            ownsSpriteVao = false;
            try
            {
                Vbo = new GLBuffer(glContext, name == null ? name : name + "_vbo");
                Vbo.CreateImmutable(capacity * SpriteVertices.SizeBytes, BufferStorageFlags.DynamicStorageBit, nint.Zero);
                SpriteVao = spriteVao;
                SpriteVao.EnsureSpriteIndices(capacity);
                Sprites = new SpriteVertices[capacity];
                SpriteVao.UseVbo(Vbo);
            }
            catch (Exception)
            {
                Dispose();
                throw;
            }
        }

        public void UseVao()
        {
            SpriteVao.UseVbo(Vbo);
            SpriteVao.Vao.Use();
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

                if(ownsSpriteVao)
                    SpriteVao?.Dispose();
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
