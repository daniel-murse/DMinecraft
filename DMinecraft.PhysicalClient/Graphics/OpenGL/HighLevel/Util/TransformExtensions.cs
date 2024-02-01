using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Util
{
    internal static class TransformExtensions
    {
        public static void CenterOrigin(this Transform transform, Vector2 size)
        {
            transform.Origin = new Vector3(size / 2);
        }
    }
}
