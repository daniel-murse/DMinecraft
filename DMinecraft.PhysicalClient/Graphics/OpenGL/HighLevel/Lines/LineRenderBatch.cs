using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites.Batches;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites.Vertices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Lines
{
    internal class LineRenderBatch
    {
        public LineRenderBatch(LineRenderer lineRenderer, LineBatch lineBatch)
        {
            LineRenderer = lineRenderer;
            LineBatch = lineBatch;
        }

        public LineRenderer LineRenderer {  get; set; }
        
        public LineBatch LineBatch { get; set; }

        public int Remaining => LineBatch.Remaining;

        public bool IsFull => LineBatch.IsFull;

        public Span<LineVertices> SubmitLinesAF(int count)
        {
            if (LineBatch.Remaining < count)
            {
                Flush();
            }
            return LineBatch.SubmitLines(count);
        }

        public void Flush()
        {
            LineRenderer.UseProgram();
            LineBatch.Flush();
        }

        public Span<LineVertices> SubmitLiness(int lineCount)
        {
            return LineBatch.SubmitLines(lineCount);
        }
    }
}
