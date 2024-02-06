using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Textures
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct UVu
    {
        //x
        public ushort U;
        //y
        public ushort V;
    }
}
