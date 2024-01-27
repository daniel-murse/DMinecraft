using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects.Data.Framebuffers
{
    internal abstract class FramebufferAttachmentImage
    {
        public abstract int Width { get; }

        public abstract int Height { get; }

        public abstract InternalFormat Format { get; }

        public abstract void Attach(GLFramebuffer framebuffer, FramebufferAttachment attachmentPoint);
    }
}
