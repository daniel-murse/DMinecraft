using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects.Data.Framebuffers
{
    internal class TextureAttachmentImage : FramebufferAttachmentImage
    {
        public TextureAttachmentImage()
        {
        }

        public TextureAttachmentImage(GLTexture texture, int mipLevel, int layer)
        {
            Texture = texture;
            MipLevel = mipLevel;
            Layer = layer;
        }

        public required GLTexture Texture { get; init; }

        public int MipLevel { get; init; }

        public int Layer { get; init; }

        public override int Width => (int)(Texture.Width / Math.Pow(2, MipLevel));

        public override int Height => (int)(Texture.Height / Math.Pow(2, MipLevel));

        public override InternalFormat Format => (InternalFormat)Texture.InternalFormat;

        public override void Attach(GLFramebuffer framebuffer, FramebufferAttachment attachmentPoint)
        {
            switch (Texture.Target)
            {
                case TextureTarget.Texture1D:
                case TextureTarget.Texture2D:
                case TextureTarget.Texture2DMultisample:
                    GL.NamedFramebufferTexture(framebuffer.Handle, attachmentPoint, Texture.Handle, MipLevel);
                    break;
                case TextureTarget.Texture3D:
                case TextureTarget.Texture1DArray:
                case TextureTarget.Texture2DArray:
                case TextureTarget.TextureCubeMapArray:
                case TextureTarget.Texture2DMultisampleArray:
                    GL.NamedFramebufferTextureLayer(framebuffer.Handle, attachmentPoint, Texture.Handle, MipLevel, Layer);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
