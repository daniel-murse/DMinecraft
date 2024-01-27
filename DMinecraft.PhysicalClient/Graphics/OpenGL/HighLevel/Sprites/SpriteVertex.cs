using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct SpriteVertex
    {
        internal static readonly int SizeBytes = Marshal.SizeOf<SpriteVertex>();
        public Vector3 Position;
        public Color4 Color;
        public Vector2 UV;
        public int Layer;
    }
}
