using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects.Data.Framebuffers
{
    internal class RenderbufferAttachmentImage : FramebufferAttachmentImage
    {
        public RenderbufferAttachmentImage()
        {
        }

        public RenderbufferAttachmentImage(GLRenderbuffer renderbuffer)
        {
            Renderbuffer = renderbuffer;
        }

        public required GLRenderbuffer Renderbuffer { get; init; }

        public override int Width => Renderbuffer.Width;

        public override int Height => Renderbuffer.Height;

        public override InternalFormat Format => (InternalFormat)Renderbuffer.Format;

        public override void Attach(GLFramebuffer framebuffer, FramebufferAttachment attachmentPoint)
        {
            GL.NamedFramebufferRenderbuffer(framebuffer.Handle, attachmentPoint, RenderbufferTarget.Renderbuffer, Renderbuffer.Handle);
        }
    }
}
