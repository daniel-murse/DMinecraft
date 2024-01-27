using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects.Data.Buffers
{
    internal class BufferRange
    {
        public GLBuffer? Buffer { get; init; }
    
        public int OffsetBytes { get; init; }

        public int SizeBytes { get; init; }
    }
}
