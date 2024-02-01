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
        /// <summary>
        /// Float3 position
        /// </summary>
        public Vector3 Position;
        /// <summary>
        /// rgba byte4 packed color
        /// </summary>
        public uint Color;
        /// <summary>
        /// Normalised x tex coord
        /// </summary>
        public ushort U;

        /// <summary>
        /// Normalised y tex coord
        /// </summary>
        public ushort V;

        /// <summary>
        /// Array layer
        /// </summary>
        public int Layer;

        /// <summary>
        /// Sampler index
        /// </summary>
        public int Index;
    }
}
