using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects.Data.VertexArrays
{
    internal class VertexAttributeFormat
    {
        public VertexAttributeFormat()
        {
        }

        public VertexAttributeFormat(int size, VertexAttribType componentType, bool isNormalised, int relativeOffset, bool isInt)
        {
            Size = size;
            ComponentType = componentType;
            IsNormalised = isNormalised;
            RelativeOffset = relativeOffset;
            IsInt = isInt;
        }

        public int Size { get; init; }

        public VertexAttribType ComponentType { get; init; }

        public bool IsNormalised { get; init; }

        public int RelativeOffset { get; init; }

        public bool IsInt { get; init; }
    }
}
