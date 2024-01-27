using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Shaders.Interfaces
{
    internal class UniformBuffer<T> : IDisposable where T : unmanaged
    {
        public GLBuffer Buffer { get; }
        public T[] Elements { get; }

        public int BufferBinding { get; }

        public UniformBuffer(GLContext glContext, int capacity, string? name = null)
        {
            try
            {
                ownsBuffer = true;
                Buffer = new GLBuffer(glContext, name);
                Elements = new T[capacity];
                Buffer.CreateImmutable(Marshal.SizeOf<T>() * capacity, BufferStorageFlags.DynamicStorageBit, 0);
                BufferBinding = glContext.BindUniformBuffer(Buffer, 0, Buffer.SizeBytes);
                boundBuffer = true;
            }
            catch (Exception)
            {
                Dispose();
                throw;
            }
        }

        // a cstrctr (with a common buffer) perhaps makes more sense
        //if the client buffer for this and other buffers use the same contiguous memory
        //so that the buffer can be updated in a single call (without copying different
        //areas of client memory to a buffer only to send it to opengl)
        //but in .net thats not really possible without the native memmory api
        //as otherwise there is no arbitrary contiguous memory from which
        //to back these buffers

        private readonly bool ownsBuffer;
        private readonly bool boundBuffer;

        public void Dispose()
        {
            if (boundBuffer)
                Buffer.Context.UnbindUniformBuffer(BufferBinding);
            if (ownsBuffer)
                ((IDisposable)Buffer).Dispose();
        }
    }
}
