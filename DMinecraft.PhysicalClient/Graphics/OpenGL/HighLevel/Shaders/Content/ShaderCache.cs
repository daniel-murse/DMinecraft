using DMinecraft.PhysicalClient.Content;
using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Shaders.Content
{
    internal class ShaderCache : ContentManager<GLShader>
    {
        public GLContext Context { get; }

        public ShaderCache(GLContext context)
        {
            Context = context;
        }
    }
}
