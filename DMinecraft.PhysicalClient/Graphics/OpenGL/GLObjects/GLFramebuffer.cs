using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects.Data.Framebuffers;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects
{
    /// <summary>
    /// <see href="https://www.khronos.org/opengl/wiki/Framebuffer"/><br/>
    /// <see href="https://www.khronos.org/opengl/wiki/Framebuffer_Object"/><br/>
    /// GL_MAX_COLOR_ATTACHMENTS is missing from one of the wikis<br/>
    /// <see href="https://stackoverflow.com/questions/29707968/get-maximum-number-of-framebuffer-color-attachments"/><br/>
    /// </summary>
    internal class GLFramebuffer : GLObject
    {
        private readonly int handle;

        public GLFramebuffer(GLContext context, string? name = null) : base(context, name)
        {
            GL.CreateFramebuffers(1, out handle);
            if(handle < 1)
            {
                throw new GLGraphicsException("Could not create framebuffer.");
            }
            UpdateGLName();

            drawBuffers = new DrawBuffersEnum[context.MaxDrawBuffers];
            colorAttachments = new FramebufferAttachmentImage?[context.MaxColorAttachments];
        }

        public int Handle => handle;

        protected override int NameHandle => Handle;

        protected override ObjectLabelIdentifier GLObjectLabelIdentifier => ObjectLabelIdentifier.Framebuffer;

        protected override void DisposeInternal()
        {
            GL.DeleteFramebuffer(Handle);
        }

        private DrawBuffersEnum[] drawBuffers;

        /// <summary>
        /// Controls what shadaer output location writes to what attachment. The indices of this array
        /// correspond to output locations and the value of the attachment represents the output
        /// color attachment.
        /// </summary>
        public DrawBuffersEnum[] DrawBuffers
        {
            get
            {
                return (DrawBuffersEnum[])drawBuffers.Clone();
            }
            set
            {
                ArgumentNullException.ThrowIfNull(value);
                ArgumentOutOfRangeException.ThrowIfLessThan(value.Length, drawBuffers.Length);

                GL.NamedFramebufferDrawBuffers(Handle, drawBuffers.Length, drawBuffers);

                drawBuffers = (DrawBuffersEnum[])value.Clone();
            }
        }

        public void AttachColor(int attatchment, FramebufferAttachmentImage image)
        {
            image.Attach(this, FramebufferAttachment.ColorAttachment0);
            colorAttachments[attatchment] = image;
        }

        public void DetachColor(int attachment)
        {
            GL.NamedFramebufferRenderbuffer(Handle, FramebufferAttachment.ColorAttachment0 + attachment, RenderbufferTarget.Renderbuffer, 0);
            colorAttachments[attachment] = null;
        }

        private FramebufferAttachmentImage?[] colorAttachments;

        /// <summary>
        /// Returns a <see cref="GLTexture"/> or a <see cref="GLRenderbuffer"/>, or <c>null</c>, depending
        /// on what is attached at the specified attachment index.
        /// </summary>
        /// <param name="attachment"></param>
        /// <returns></returns>
        public FramebufferAttachmentImage? GetColorAttachment(int attachment)
        {
            return colorAttachments[attachment];
        }

        private ReadBufferMode readBuffer;

        /// <summary>
        /// Where read operations for this framebuffer get the image (2d pixel array) to read from.
        /// Is one of the color attachment points, or GL_NONE (reading will be disabled and fail).
        /// </summary>
        public ReadBufferMode ReadBuffer
        {
            get { return readBuffer; }
            set
            {
                GL.NamedFramebufferReadBuffer(Handle, readBuffer);
                readBuffer = value;
            }
        }

        private FramebufferAttachmentImage? depthAttachment;

        private FramebufferAttachmentImage? stencilAttachment;

        public void AttachDepth(FramebufferAttachmentImage image)
        {
            image.Attach(this, FramebufferAttachment.DepthAttachment);
            depthAttachment = image;
        }

        public void AttachStencil(FramebufferAttachmentImage image)
        {
            image.Attach(this, FramebufferAttachment.StencilAttachment);
            stencilAttachment = image;
        }

        public void AttachDepthStencil(FramebufferAttachmentImage image)
        {
            image.Attach(this, FramebufferAttachment.DepthStencilAttachment);
            depthAttachment = image;
            stencilAttachment = image;
        }

        public void DetachDepth()
        {
            depthAttachment = null;
            GL.NamedFramebufferRenderbuffer(Handle, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, 0);
        }

        public void DetachStencil()
        {
            stencilAttachment = null;
            GL.NamedFramebufferRenderbuffer(Handle, FramebufferAttachment.Stencil, RenderbufferTarget.Renderbuffer, 0);

        }

        public void DetachDepthStencil()
        {
            depthAttachment = null;
            stencilAttachment = null;
            GL.NamedFramebufferRenderbuffer(Handle, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, 0);
        }

        /// <summary>
        /// Could eq <see cref="GetStencilAttachment"/> if the same image (with a packed depth-stencil format)
        /// is used for the attachments.
        /// </summary>
        /// <returns></returns>
        public FramebufferAttachmentImage? GetDepthAttachment() 
        {
            return depthAttachment;
        }

        /// <summary>
        /// Could eq <see cref="GetDepthAttachment"/> if the same image (with a packed depth-stencil format)
        /// is used for the attachments.
        /// </summary>
        /// <returns></returns>
        public FramebufferAttachmentImage? GetStencilAttachment()
        {
            return stencilAttachment;
        }

        /// <summary>
        /// Returns the object which is the image of both the depth and stencil attachment, or null if
        /// the depth and stencil attachments are bound to different images. Null can also be returned
        /// if both attachments are unbound.
        /// </summary>
        /// <returns></returns>
        public FramebufferAttachmentImage? GetDepthStencilAttachment()
        {
            return stencilAttachment == depthAttachment ? stencilAttachment : null;
        }

        public bool CheckIsComplete()
        {
            //dont care about 2nd param
            //https://registry.khronos.org/OpenGL-Refpages/gl4/html/glCheckFramebufferStatus.xhtml
            FramebufferStatus status = GL.CheckNamedFramebufferStatus(Handle, FramebufferTarget.Framebuffer);
            return status == FramebufferStatus.FramebufferComplete;
        }

        public void AttachColorTexture2D(int attachment, GLTexture texture)
        {
            AttachColor(attachment, new TextureAttachmentImage() { Texture = texture });
        }

        public void AttachDepthStencilRenderBuffer(GLRenderbuffer renderbuffer)
        {
            AttachDepthStencil(new RenderbufferAttachmentImage() { Renderbuffer = renderbuffer });
        }

        public void SetDrawBuffersSequential()
        {
            DrawBuffers = Enumerable.Range(0, Context.MaxDrawBuffers)
                .Select(p => p < Context.MaxColorAttachments ?
                (DrawBuffersEnum)(FramebufferAttachment.ColorAttachment0 + p) :
                DrawBuffersEnum.None).ToArray();
        }
    }
}
