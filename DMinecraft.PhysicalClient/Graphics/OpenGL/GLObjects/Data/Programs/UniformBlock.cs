using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects.Data.Programs
{
    internal class UniformBlock
    {
        public required int SizeBytes { get; init; }

        public required IReadOnlyCollection<UniformBlockMember> Members { get; init;}

        public required string Name { get; init; }
    
        public required int Index { get; init; }
    }
}
