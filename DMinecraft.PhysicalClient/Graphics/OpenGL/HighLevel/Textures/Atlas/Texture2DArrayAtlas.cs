using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Textures.Atlas
{
    internal class Texture2DArrayAtlas : IDisposable
    {
        private bool disposedValue;

        public Texture2DArrayAtlas(GLContext context, Texture2DArrayAtlasOptions options, string? name = null)
        {
            Options = options;
            Texture = new GLTexture(context, TextureTarget.Texture2DArray, name);
            try
            {
                Texture.CreateImmutable3D(options.MipLevels, options.Width, options.Height, options.Depth, (SizedInternalFormat)options.InternalFormat);
            }
            catch (Exception)
            {
                Texture.Dispose();
                throw;
            }
            freeLayers = new Queue<int>();
        }

        public required Texture2DArrayAtlasOptions Options { get; init; }

        public GLTexture Texture { get; }

        public int ImageCount { get; private set; }

        public bool IsFull => ImageCount == Texture.Depth;

        public Texture2DArrayAtlasItem AddImage(PixelFormat pixelFormat, PixelType pixelType, Span<byte> data)
        {
            int layer;
            if(useFreeLayers)
            {
                if (!freeLayers.TryDequeue(out layer))
                    throw new GLGraphicsException("Texture2DArray atlas full!");
            }
            else if(ImageCount == Texture.Depth)
            {
                throw new GLGraphicsException("Texture2DArray atlas full!");
            }
            else
            {
                layer = ImageCount++;
                maxLayerAdded = layer;
            }

            Texture.SubImage3D(0, 0, 0, layer, Options.Width, Options.Height, 1, pixelFormat, pixelType, data);

            if (maxLayerAdded == Texture.Depth)
                useFreeLayers = true;

            return new Texture2DArrayAtlasItem(this, layer);
        }

        private Queue<int> freeLayers;

        private bool useFreeLayers;

        private int maxLayerAdded;

        public void FreeImage(Texture2DArrayAtlasItem item)
        {
            ImageCount--;
            freeLayers.Enqueue(item.Layer);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                Texture.Dispose();
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Texture2DArrayAtlas()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
