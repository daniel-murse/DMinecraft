using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects;
using OpenTK.Graphics.OpenGL4;
using StbImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Util
{
    internal static class Loading
    {
        public static GLTexture LoadTexture2DFromFile(GLContext context, string path, int mipLevels = 1, SizedInternalFormat internalFormat = SizedInternalFormat.Rgb8)
        {
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                StbImage.stbi_set_flip_vertically_on_load(1);
                ImageResult imageResult = ImageResult.FromStream(fileStream);

                GLTexture texture = new GLTexture(context, TextureTarget.Texture2D, Path.GetFileName(path));

                texture.CreateImmutable2D(mipLevels, texture.Width, texture.Height, internalFormat);
                texture.SubImage2D(0, 0, 0, texture.Width, texture.Height, PixelFormat.Rgba, PixelType.UnsignedByte, imageResult.Data.AsSpan());

                return texture;
            }
        }

        public static GLShader LoadShaderFromFile(GLContext context, string path, ShaderType shaderType)
        {
            GLShader shader = new GLShader(shaderType, context, Path.GetFileName(path));
            try
            {
                shader.Compile(File.ReadAllText(path));
            }
            catch (GLGraphicsException)
            {
                shader.Dispose();
                throw;
            }
            return shader;
        }
    }
}
