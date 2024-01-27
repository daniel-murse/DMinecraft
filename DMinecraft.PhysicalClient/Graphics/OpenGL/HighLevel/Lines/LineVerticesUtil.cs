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
        public static void Compute(Vector3 a, Vector3 b, Color4 color, out LineVertices line)
        {
            line.A.Position = a;
            line.B.Position = b;
            line.A.Color = color;
            line.B.Color = color;
        }

        public static void TestDraw(LineBatch batch)
        {
            if (batch.IsFull)
            {
                batch.Draw();
                batch.Clear();
            }
            var lines = batch.SubmitLines(2);
            LineVerticesUtil.Compute(new Vector3(-1, -1, 0), new Vector3(1, 1, 0), Color4.Violet, out lines[0]);
            LineVerticesUtil.Compute(new Vector3(-1, 1, 0), new Vector3(1, -1, 0), Color4.BlueViolet, out lines[1]);
        }
    }
}
