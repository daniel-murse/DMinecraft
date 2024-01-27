using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites.Packed
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct SpriteVertex
    {
        public static readonly int SizeBytes = Marshal.SizeOf<SpriteVertex>();
        public Vector3 Position;
        public uint Color;
        public byte U;
        public byte V;
        public int Layer;
    }
}
