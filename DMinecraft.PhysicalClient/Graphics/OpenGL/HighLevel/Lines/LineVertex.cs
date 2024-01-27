using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Lines
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct LineVertex
    {
        public Vector3 Position;
        public Color4 Color;

        public static readonly int SizeBytes = Marshal.SizeOf<LineVertex>();
    }
}
