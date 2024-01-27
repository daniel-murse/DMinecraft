using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects
{
    /// <summary>
    /// <see href="https://www.khronos.org/opengl/wiki/Buffer_Object"/>
    /// </summary>
    internal class GLBuffer : GLObject
    {
        private readonly int handle;

        public int Handle => handle;
        public GLBuffer(GLContext context, string? name = null) : base(context, name)
        {
            GL.CreateBuffers(1, out handle);
            if (handle < 1)
            {
                throw new GLGraphicsException("Could not create buffer.");
            }
            UpdateGLName();
        }

        protected override int NameHandle => Handle;

        protected override ObjectLabelIdentifier GLObjectLabelIdentifier => ObjectLabelIdentifier.Buffer;

        protected override void DisposeInternal()
        {
            GL.DeleteBuffer(Handle);
        }

        public int SizeBytes { get; private set; }

        public bool IsImmutable { get; private set; }

        /// <summary>
        /// Makes the buffer immutable.
        /// </summary>
        /// <param name="sizeBytes">The size in bytes of the buffer.</param>
        /// <param name="flags"><see cref="BufferStorageFlags.DynamicStorageBit"/>
        /// to be able to call glBufferSubData.</param>
        public void CreateImmutable<T>(int sizeBytes, BufferStorageFlags flags, Span<T> data) where T : unmanaged
        {
            EnsureMutable();
            GL.NamedBufferStorage(Handle, sizeBytes, ref data[0], BufferStorageFlags.DynamicStorageBit);
            SizeBytes = sizeBytes;
            IsImmutable = true;
            Flags = flags;
        }

        public void CreateImmutable(int sizeBytes, BufferStorageFlags flags, nint data)
        {
            EnsureMutable();
            GL.NamedBufferStorage(Handle, sizeBytes, data, BufferStorageFlags.DynamicStorageBit);
            SizeBytes = sizeBytes;
            IsImmutable = true;
            Flags = flags;
        }

        private void EnsureMutable()
        {
            if (IsImmutable)
            {
                throw new GLGraphicsException("Buffer is immutable");
            }
        }

        public void CreateMutable(int sizeBytes, Span<byte> data, BufferUsageHint usageHint)
        {
            GL.NamedBufferData(Handle, sizeBytes, ref data[0], usageHint);
            SizeBytes = sizeBytes;
            UsageHint = usageHint;
        }

        public BufferUsageHint UsageHint { get; private set; }

        public BufferStorageFlags Flags { get; private set; }

        public void SubData<T>(int offset, int sizeBytes, Span<T> data) where T : unmanaged
        {
            GL.NamedBufferSubData(Handle, offset, sizeBytes, ref data[0]);
        }

        public nint Map(BufferAccess access)
        {
            return GL.MapNamedBuffer(Handle, access);
        }

        public bool Unmap()
        {
            return GL.UnmapNamedBuffer(Handle);
        }
    }
}
