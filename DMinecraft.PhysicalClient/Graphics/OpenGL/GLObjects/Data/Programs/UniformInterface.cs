using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects.Data.Programs
{
    internal class UniformInterface
    {
        public required IReadOnlyList<UniformResource> Uniforms { get; init; }
        public required IReadOnlyList<UniformBlock> UniformBlocks { get; init; }

        public UniformResource GetResource(int location, int type, int arraySize)
        {
            return Uniforms.Where(p => p.Location == location && p.Type == type && p.ArraySize == arraySize).FirstOrDefault() ?? throw new GLGraphicsException();
        }
    }
}
