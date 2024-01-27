using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Voxels.Loading
{
    internal class ModelBuilder
    {
        private Memory<Vertex> vertices;

        public ModelBuilder(bool useIndices, int capacity)
        {
            UseIndices = useIndices;
            Capacity = capacity;
            vertices = new Memory<Vertex>(new Vertex[capacity]);
        }

        public bool UseIndices { get; init; }
        public int Capacity { get; }
        public int VertexCount { get; }

        public void AddTriangle(Span<Vertex> vertices)
        {

        }

        public void AddQuad(Span<Vertex> quad)
        {
            if (UseIndices)
            {
            }
            else
            {

            }
        }
    }
}
