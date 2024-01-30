using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Textures.Atlas;
using FreeTypeBinding;
using HarfBuzzSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Text.Content
{
    internal unsafe class FontBuilder : IDisposable
    {
        private FT_FaceRec_* ftFace;
        private HarfBuzzSharp.Face hbFace;
        private bool disposedValue;

        public FontBuilder(FT_FaceRec_* ftFace, Face hbFace)
        {
            this.ftFace = ftFace;
            this.hbFace = hbFace;
        }

        //only 1 at a time ft isnt thread safe on the same face
        public FontCreationCoroutine BuildCreationCoroutine(int sizePixels, uint glyphStart, uint glyphEnd, bool isSdf, GLContext glContext)
        {
            return new FontCreationCoroutine(ftFace, hbFace, new (uint start, uint end)[] { (glyphStart, glyphEnd) }, sizePixels, isSdf, glContext);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                if (ftFace != null)
                    FT.FT_Done_Face(ftFace);
                hbFace.Dispose();

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~FontBuilder()
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
