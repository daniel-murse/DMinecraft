using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects
{
    internal abstract class GLObject : IDisposable
    {
        public GLContext Context { get; }

        public string? Name
        {
            get => name; set
            {
                name = value;
                if (NameHandle == 0)
                    return;
                UpdateGLName();
            }
        }

        protected void UpdateGLName()
        {
            if (name != null)
            {
                GL.ObjectLabel(GLObjectLabelIdentifier, NameHandle, name.Length, name);
            }
            else
            {
                GL.ObjectLabel(GLObjectLabelIdentifier, NameHandle, 0, null);
            }
        }

        protected GLObject(GLContext context, string? name = null)
        {
            Context = context;
            Name = name;
        }

        protected abstract int NameHandle { get; }

        protected abstract void DisposeInternal();

        protected abstract ObjectLabelIdentifier GLObjectLabelIdentifier { get; }

        protected bool disposedValue;
        private string? name;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }
                DisposeInternal();
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~GLObject()
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
