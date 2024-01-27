using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects;
using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects.Data.Programs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Shaders.Interfaces
{
    internal class UniformBlockInterface<T> : IDisposable where T : unmanaged
    {
        public GLProgram Program { get; }

        public UniformBlock Block { get; }

        public UniformBuffer<T> Buffer { get; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
