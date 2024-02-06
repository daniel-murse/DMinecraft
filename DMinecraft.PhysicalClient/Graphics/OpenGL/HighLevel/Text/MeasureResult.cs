using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Text
{
    internal struct MeasureResult
    {
        public Vector4 RenderBounds;
        public Vector2 RenderSize;

        public Vector4 FormatBounds;
        public Vector2 FormatSize;
    }
}
