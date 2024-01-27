using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects
{

    [Serializable]
    public class GLGraphicsException : Exception
    {
        public GLGraphicsException() { }
        public GLGraphicsException(string message) : base(message) { }
        public GLGraphicsException(string message, Exception inner) : base(message, inner) { }
    }
}
