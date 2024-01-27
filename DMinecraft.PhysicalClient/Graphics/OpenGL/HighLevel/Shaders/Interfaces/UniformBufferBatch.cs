using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Shaders.Interfaces
{
    internal class UniformBufferBatch<T> : IDisposable where T : unmanaged
    {
        private readonly bool ownsBuffer;

        public UniformBuffer<T> Buffer { get; }

        public UniformBufferBatch(GLContext glContext, int capacity, string? name = null)
        {
            ownsBuffer = true;
            Buffer = new UniformBuffer<T>(glContext, capacity, name);
        }

        public void Dispose()
        {
            if (ownsBuffer)
                ((IDisposable)Buffer).Dispose();
        }

        public int Count { get; private set; }

        public int Capacity => Buffer.Elements.Length;

        public int Remaining => Capacity - Count;

        public Span<T> Submit(int count)
        {
            if (Remaining < count)
            {
                throw new GLGraphicsException("Clip plane capacity exceeded.");
            }
            var span = Buffer.Elements.AsSpan(Count, count);
            Count += count;
            return span;
        }

        public void Clear()
        {
            Count = 0;
        }

        public void Update()
        {
            Buffer.Buffer.SubData(0, Marshal.SizeOf<T>() * Count, Buffer.Elements.AsSpan());
        }
    }
}
