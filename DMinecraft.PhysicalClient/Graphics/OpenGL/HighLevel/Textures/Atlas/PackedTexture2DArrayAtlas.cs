using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Textures.Atlas
{
    internal class PackedTexture2DArrayAtlas : IDisposable
    {
        public GLTexture Texture { get; }
        public PackedTexture2DArrayAtlasOptions Options { get; }

        public PackedTexture2DArrayAtlas(GLContext glContext, PackedTexture2DArrayAtlasOptions options)
        {
            Options = options;
            Texture = new GLTexture(glContext, TextureTarget.Texture2DArray);
            try
            {
                Texture.CreateImmutable3D(options.MipLevels, options.TextureWidth, options.TextureWidth, options.Depth, (SizedInternalFormat)options.InternalFormat);
            }
            catch (Exception)
            {
                Dispose();
                throw;
            }
            
        }

        public void Dispose()
        {
            ((IDisposable)Texture).Dispose();
        }

        public int CurrentLayer { get; private set; }

        public int CurrentRow { get; private set; }

        public int CurrentCol { get; private set;}
    
        public PackedTexture2DArrayAtlasItem AddImage(PixelFormat pixelFormat, PixelType pixelType, int rows, int cols, Span<byte> data)
        {
            //row major
            if (CurrentCol == Options.Cols)
            {
                CurrentRow++;
                CurrentCol = 0;
            }
            if(CurrentRow == Options.Rows)
            {
                CurrentLayer++;
                CurrentRow = 0;
            }
            if (CurrentLayer == Options.Depth)
                throw new GLGraphicsException("Atlas full");

            int x = CurrentCol * Options.ImageWidth;
            int y = CurrentRow * Options.ImageHeight;
            Texture.SubImage3D(0, x, y, CurrentLayer, cols, rows, 1, pixelFormat, pixelType, data);

            var coordinates = new PackedTexture2DArrayAtlasItem(
                this,
                (byte)(x / (float)Texture.Width * 255),
                (byte)(y / (float)Texture.Height * 255),
                (byte)((x + cols) / (float)Texture.Width * 255),
                (byte)((y + rows) / (float)Texture.Width * 255),
                CurrentLayer);
            CurrentCol++;
            return coordinates;
        }
    }
}
