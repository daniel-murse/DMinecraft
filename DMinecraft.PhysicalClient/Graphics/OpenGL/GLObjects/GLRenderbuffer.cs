using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects
{
    internal class GLRenderbuffer : GLObject
    {
        private readonly int handle;

        public int Handle => handle;
        public GLRenderbuffer(GLContext context, string? name = null) : base(context, name)
        {
            GL.CreateRenderbuffers(1, out handle);
            if(handle < 1)
            {
                throw new GLGraphicsException("Could not create render buffer.");
            }
            UpdateGLName();

        }

        protected override int NameHandle => Handle;

        protected override ObjectLabelIdentifier GLObjectLabelIdentifier => ObjectLabelIdentifier.Renderbuffer;

        protected override void DisposeInternal()
        {
            GL.DeleteRenderbuffer(handle);
        }

        public RenderbufferStorage Format { get; private set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public void SetStorage(RenderbufferStorage format, int width, int height, int samples = 0)
        {
            GL.NamedRenderbufferStorageMultisample(Handle, samples, format, width, height);
            Format = format;
            Width = width;
            Height = height;
        }
    }
}
