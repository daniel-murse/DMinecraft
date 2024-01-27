using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects.Data.Programs
{
    internal class UniformBlockMember
    {
        public int Type { get; init; }

        public int ArraySize { get; init; }

        public string Name { get; init; }

        public int OffsetBytes { get; init; }

        public int ArrayStrideBytes { get; init; }
    }
}
