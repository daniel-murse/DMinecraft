using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Shaders.Interfaces
{
    internal abstract class IntAbstractUniformInterface : AbstractUniformInterface<int>
    {
        protected IntAbstractUniformInterface(GLProgram program, int location) : base(program, location)
        {
        }

        public override void Set(Span<int> values)
        {
            GL.ProgramUniform1(Program.Handle, Uniform.Location, values.Length, ref values[0]);
        }

        public override void Set(ref int value)
        {
            GL.ProgramUniform1(Program.Handle, Uniform.Location, value);
        }

        public override void Set(int value)
        {
            GL.ProgramUniform1(Program.Handle, Uniform.Location, value);
        }
    }
}
