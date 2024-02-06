using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects;
using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects.Data.VertexArrays;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites.Vertices;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites.Meshes
{
    //uses a single vao for sprite meshes,
    internal class SpriteMeshVao : IDisposable
    {
        public SpriteMeshVao(GLContext glContext, SpriteVertexDeclaration vertexDeclaration, int capacity, string? name = null)
        {
            VertexDeclaration = vertexDeclaration;
            try
            {
                Vao = new GLVertexArray(glContext, name);
                EnsureSpriteIndices(capacity);
                ConfigureVao();
            }
            catch (Exception)
            {
                Dispose();
                throw;
            }
        }

        public GLVertexArray Vao { get; }

        public SpriteVertexDeclaration VertexDeclaration { get; }

        public GLBuffer Ebo { get; private set; }

        public int SpriteCount { get; }

        public void UseVbo(GLBuffer buffer)
        {
            if(Vao.GetVertexBufferBinding(0).Buffer != buffer)
            {
                Vao.SetVertexBufferBinding(0, new VertexBufferBinding(SpriteVertex.SizeBytes, 0, buffer));
            }
        }

        public void EnsureSpriteIndices(int spriteCount)
        {
            if(spriteCount > SpriteCount)
            {
                Ebo?.Dispose();
                Ebo = new GLBuffer(Vao.Context, Ebo?.Name);
                Ebo.CreateImmutable(spriteCount * SpriteVertices.SizeBytes, BufferStorageFlags.None, ComputeSpriteIndices(spriteCount).AsSpan());
                Vao.IndexBuffer = Ebo;
            }
        }

        public ushort[] ComputeSpriteIndices(int spriteCount)
        {
            ushort[] indices = new ushort[spriteCount * 6];

            for (int i = 0; i < spriteCount; i++)
            {

                //bl tl br tr
                int spriteIndex = i * 6;
                int vertIndex = i * 4;
                indices[spriteIndex] = (ushort)vertIndex;//bl
                indices[spriteIndex + 1] = (ushort)(vertIndex + 1);//tl
                indices[spriteIndex + 2] = (ushort)(vertIndex + 2);//br
                indices[spriteIndex + 3] = (ushort)(vertIndex + 1);//tl
                indices[spriteIndex + 4] = (ushort)(vertIndex + 3); //tr
                indices[spriteIndex + 5] = (ushort)(vertIndex + 2);//br
            }

            return indices;
        }

        public void Dispose()
        {
            Vao?.Dispose();
            Ebo?.Dispose();
        }

        private void ConfigureVao()
        {
            //Vao.SetVertexBufferBinding(0, new VertexBufferBinding(SpriteVertex.SizeBytes, 0, Vbo));
            Vao.SetVertexAttributeFormat(VertexDeclaration.PositionLocation, new VertexAttributeFormat(3, VertexAttribType.Float, false, 0, false));
            Vao.SetVertexAttributeFormat(VertexDeclaration.ColorLocation, new VertexAttributeFormat(1, VertexAttribType.UnsignedInt, false, 12, true));
            Vao.SetVertexAttributeFormat(VertexDeclaration.UVLocation, new VertexAttributeFormat(2, VertexAttribType.UnsignedShort, true, 16, false));
            Vao.SetVertexAttributeFormat(VertexDeclaration.LayerIndexLocation, new VertexAttributeFormat(2, VertexAttribType.Int, false, 20, true));
            Vao.SetVertexAttributeBinding(VertexDeclaration.PositionLocation, 0);
            Vao.SetVertexAttributeBinding(VertexDeclaration.ColorLocation, 0);
            Vao.SetVertexAttributeBinding(VertexDeclaration.UVLocation, 0);
            Vao.SetVertexAttributeBinding(VertexDeclaration.LayerIndexLocation, 0);
            Vao.SetVertexAttributeEnabled(VertexDeclaration.PositionLocation, true);
            Vao.SetVertexAttributeEnabled(VertexDeclaration.ColorLocation, true);
            Vao.SetVertexAttributeEnabled(VertexDeclaration.UVLocation, true);
            Vao.SetVertexAttributeEnabled(VertexDeclaration.LayerIndexLocation, true);
        }
    }
}
