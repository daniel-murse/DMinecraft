using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Lines
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct LineVertices
    {
        public static readonly int SizeBytes = Marshal.SizeOf<LineVertices>();
        public LineVertex A;
        public LineVertex B;
    }
}
