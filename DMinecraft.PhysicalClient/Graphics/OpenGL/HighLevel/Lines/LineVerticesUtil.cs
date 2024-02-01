using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Lines
{
    internal static class LineVerticesUtil
    {
        public static void Compute(ref this LineVertices line, Vector3 a, Vector3 b, Color4 color)
        {
            line.A.Position = a;
            line.B.Position = b;
            line.A.Color = color;
            line.B.Color = color;
        }

        public static void Compute(ref this LineVertices line, Vector2 start, Vector2 end, Color4 color)
        {
            line.Compute(new Vector3(start), new Vector3(end), color);
        }

        public static void TestDraw(LineBatch batch)
        {
            if (batch.IsFull)
            {
                batch.Draw();
                batch.Clear();
            }
            var lines = batch.SubmitLines(2);
            lines[0].Compute(new Vector3(-1, -1, 0), new Vector3(1, 1, 0), Color4.Violet);
            lines[1].Compute(new Vector3(1, 1, 0), new Vector3(-1, -1, 0), Color4.BlueViolet);
        }
    }
}
