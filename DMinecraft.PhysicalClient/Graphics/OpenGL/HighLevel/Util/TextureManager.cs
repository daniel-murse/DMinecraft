using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Util
{
    internal class TextureManager : IDisposable
    {
        private Dictionary<string, GLTexture> textures;
        private bool disposedValue;

        public TextureManager(GLContext context, string rootDirectory)
        {
            textures = new Dictionary<string, GLTexture>();
            GLContext = context;
            RootDirectory = rootDirectory;
        }

        public GLContext GLContext { get; }
        public string RootDirectory { get; init; }

        public GLTexture GetOrLoadTexture2D(string filename)
        {
            if (textures.TryGetValue(filename, out var texture)) return texture;
            texture = Loading.LoadTexture2DFromFile(GLContext, Path.Combine(RootDirectory, filename));
            textures[filename] = texture;
            return texture;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                foreach (var texture in textures.Values)
                {
                    texture.Dispose();
                }
                textures.Clear();
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~TextureManager()
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
