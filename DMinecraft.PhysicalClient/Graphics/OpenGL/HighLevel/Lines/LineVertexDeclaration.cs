using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects;
using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects.Data.Programs;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Lines
{
    internal class LineVertexDeclaration
    {
        public required int PositionLocation { get; init; }
        public required int ColorLocation { get; init; }

        [SetsRequiredMembers]
        public LineVertexDeclaration(InputInterface input)
        {
            PositionLocation = input.Resources.Where(p => p.Location == 0 && p.Type == (int)All.FloatVec3 && p.ArraySize == 1).FirstOrDefault()?.Location ?? throw new GLGraphicsException("Position not found at location 0.");
            ColorLocation = input.Resources.Where(p => p.Location == 1 && p.Type == (int)All.FloatVec4 && p.ArraySize == 1).FirstOrDefault()?.Location ?? throw new GLGraphicsException("Color not found at location 1.");
        }
    }
}
