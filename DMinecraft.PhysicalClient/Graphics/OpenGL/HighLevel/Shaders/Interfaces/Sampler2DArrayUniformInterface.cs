using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Shaders.Interfaces
{
    internal class Sampler2DArrayUniformInterface : IntAbstractUniformInterface
    {
        public Sampler2DArrayUniformInterface(GLProgram program, int location) : base(program, location)
        {
        }

        public override int GLType => (int)All.Sampler2DArray;
    }
}
