using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Util
{
    internal static class ColorExtensions
    {
        public static uint ToRgba(this Color4 color)
        {
            //https://registry.khronos.org/OpenGL-Refpages/gl4/html/unpackUnorm.xhtml
            //check order of components
            return ((uint)(color.R * 255f) << 24) | ((uint)(color.G * 255f) << 16) | ((uint)(color.B * 255f) << 8) | ((uint)(color.A * 255f));
        }
    }
}
