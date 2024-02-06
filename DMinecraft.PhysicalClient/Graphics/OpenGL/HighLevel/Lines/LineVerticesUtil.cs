using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Util;
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
        public static void Compute(ref this LineVertices line, Vector3 a, Vector3 b, Color4 color, Transform? transform = null)
        {
            if(transform != null)
            {
                Vector3.TransformVector(a, in transform.RefMatrix, out a);
                Vector3.TransformVector(b, in transform.RefMatrix, out b);
                a += transform.Position;
                b += transform.Position;
            }
            
            line.A.Position = a;
            line.B.Position = b;
            line.A.Color = color;
            line.B.Color = color;
        }

        public static void Compute(ref this LineVertices line, Vector2 start, Vector2 end, Color4 color, Transform? transform = null)
        {
            line.Compute(new Vector3(start), new Vector3(end), color, transform);
        }

        public static void ComputeQuad(this Span<LineVertices> lines, Vector2 bl, Vector2 tr, Color4 color, Transform? transform = null)
        {
            if (lines.Length != 4)
                throw new ArgumentException();
            lines[0].Compute(new Vector2(bl.X, bl.Y), new Vector2(bl.X, tr.Y), color, transform);
            lines[1].Compute(new Vector2(bl.X, bl.Y), new Vector2(tr.X, bl.Y), color, transform);
            lines[2].Compute(new Vector2(tr.X, tr.Y), new Vector2(tr.X, bl.Y), color, transform);
            lines[3].Compute(new Vector2(tr.X, tr.Y), new Vector2(bl.X, tr.Y), color, transform);
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
