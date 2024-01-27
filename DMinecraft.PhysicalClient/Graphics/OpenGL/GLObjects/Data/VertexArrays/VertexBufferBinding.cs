using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects.Data.VertexArrays
{
    internal class VertexBufferBinding
    {
        public VertexBufferBinding()
        {
        }

        [SetsRequiredMembers]
        public VertexBufferBinding(int strideBytes, int offsetBytes, GLBuffer buffer)
        {
            StrideBytes = strideBytes;
            OffsetBytes = offsetBytes;
            Buffer = buffer;
        }

        public int StrideBytes { get; init; }

        public int OffsetBytes { get; init; }

        public required GLBuffer Buffer { get; init; }
    }
}
