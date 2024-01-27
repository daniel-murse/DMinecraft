using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Textures
{
    internal abstract class RgbaImage2D
    {
        public abstract int Width { get; }

        public abstract int Height { get; }

        public abstract Color4 GetPixel(int x, int y);

        public bool IsPixel(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }
    }
}
