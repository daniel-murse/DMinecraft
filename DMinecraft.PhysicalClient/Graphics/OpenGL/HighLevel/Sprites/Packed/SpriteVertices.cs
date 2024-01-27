using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites.Packed
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct SpriteVertices
    {
        public SpriteVertex BottomLeft;
        public SpriteVertex TopLeft;
        public SpriteVertex BottomRight;
        public SpriteVertex TopRight;

        public static readonly int SizeBytes = SpriteVertex.SizeBytes * 4;
    }
}
