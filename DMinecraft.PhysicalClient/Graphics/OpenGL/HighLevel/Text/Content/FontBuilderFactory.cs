using FreeTypeBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Text.Content
{
    internal unsafe class FontBuilderFactory : IDisposable
    {
        private FT_LibraryRec_* ftLib;

        private Dictionary<string, HarfBuzzSharp.Blob> hbBlobs;

        private bool disposedValue;

        public string ContentRoot { get; }

        public FontBuilderFactory(string contentRoot)
        {
            FT_LibraryRec_* stackftLib;
            if (FT_Error.FT_Err_Ok != FT.FT_Init_FreeType(&stackftLib))
                throw new FontException();
            ftLib = stackftLib;
            hbBlobs = new Dictionary<string, HarfBuzzSharp.Blob> ();
            ContentRoot = contentRoot;
        }

        public FontBuilder CreateBuilder(string path, int index = 0)
        {
            var fullPath = Path.Combine(ContentRoot, path);

            if(!hbBlobs.TryGetValue(path, out var blob))
            {
                blob = HarfBuzzSharp.Blob.FromFile(fullPath);
                hbBlobs.Add(path, blob);
            }

            var hbFace = new HarfBuzzSharp.Face(blob, index);
            FT_FaceRec_* ftFace;
            var bytes = Encoding.ASCII.GetBytes(fullPath);
            fixed(byte* ptr = bytes)
            {
                //to be deleted by callers
                FT.FT_New_Face(ftLib, ptr, index, &ftFace);
            }

            return new FontBuilder(ftFace, hbFace);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }
                if(ftLib != null)
                    FT.FT_Done_FreeType(ftLib);
                foreach (var item in hbBlobs.Values)
                {
                    item.Dispose();
                }
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~FontBuilderBuilder()
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
