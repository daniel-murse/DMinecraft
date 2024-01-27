using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Shaders.Interfaces
{
    internal class Matrix4UniformInterface : AbstractUniformInterface<Matrix4>
    {
        public Matrix4UniformInterface(GLProgram program, int location) : base(program, location)
        {
        }

        public override int GLType => (int)All.FloatMat4;

        public override void Set(Span<Matrix4> values)
        {
            GL.ProgramUniformMatrix4(Program.Handle, Uniform.Location, values.Length, false, ref values[0].Row0.X);
        }

        public override void Set(ref Matrix4 value)
        {
            GL.ProgramUniformMatrix4(Program.Handle, Uniform.Location, false, ref value);
        }

        public override void Set(Matrix4 value)
        {
            Set(ref value);
        }
    }
}
