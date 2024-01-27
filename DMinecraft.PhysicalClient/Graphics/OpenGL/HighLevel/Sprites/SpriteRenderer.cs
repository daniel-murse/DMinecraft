using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Shaders.Interfaces;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites
{
    internal class SpriteRenderer
    {
        public GLProgram Program { get; }

        public Matrix4UniformInterface Transform { get; }

        public Sampler2DArrayUniformInterface Albedo { get; }

        public SpriteRenderer(GLProgram program)
        {
            Program = program;
            Transform = new Matrix4UniformInterface(program, program.Interface.Uniform.GetResource(0, (int)All.FloatMat4, 1).Location);
            Albedo = new Sampler2DArrayUniformInterface(program, program.Interface.Uniform.GetResource(0, (int)All.Sampler2DArray, 1).Location);
        }
    }
}
