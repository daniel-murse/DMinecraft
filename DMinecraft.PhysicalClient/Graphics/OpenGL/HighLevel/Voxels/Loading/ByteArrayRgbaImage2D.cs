using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Textures;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Voxels.Loading
{
    internal class ByteArrayRgbaImage2D : RgbaImage2D
    {
        public ByteArrayRgbaImage2D(byte[] data, int width, int height)
        {
            Data = data;
            Width = width;
            Height = height;
        }

        public byte[] Data { get; private set; }

        public override int Width {get; }

        public override int Height { get; }

        public override Color4 GetPixel(int x, int y)
        {
            return new Color4(Data[Width * y + x], Data[Width * y + x + 1], Data[Width * y + x + 2], Data[Width * y + x + 3]);
        }
    }
}
