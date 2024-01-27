using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects;
using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects.Data.Programs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Shaders.Interfaces
{
    internal abstract class AbstractUniformInterface<T> where T : unmanaged
    {
        public AbstractUniformInterface(GLProgram program, int location)
        {
            Program = program;
            if (!Program.IsLinked)
                throw new GLGraphicsException();
            Uniform = Program.Interface.Uniform.Uniforms.Where(p => p.Location == location).FirstOrDefault() ?? throw new GLGraphicsException();
            if (Uniform.Type != GLType)
                throw new GLGraphicsException();
        }

        public GLProgram Program { get; }

        public UniformResource Uniform { get; }

        public abstract int GLType { get; }

        public abstract void Set(Span<T> values);

        public abstract void Set(ref T value);

        public abstract void Set(T value);
    }
}
