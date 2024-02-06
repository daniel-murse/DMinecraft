using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Shaders.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Lines
{
    internal class LineRenderer
    {
        public GLProgram Program { get; }

        public Matrix4UniformInterface Transform { get; }

        public LineVertexDeclaration VertexDeclaration { get; }

        public LineRenderer(GLProgram program)
        {
            VertexDeclaration = new LineVertexDeclaration(program.Interface.Input);
            Transform = new Matrix4UniformInterface(program, 0);
            Program = program;
        }

        internal void UseProgram()
        {
            Program.Use();
        }
    }
}
