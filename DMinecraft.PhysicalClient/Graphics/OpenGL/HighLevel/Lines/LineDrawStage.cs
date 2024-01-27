using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Pipeline;
using OpenTK.Graphics.ES30;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Lines
{
    internal delegate void LineDrawAction(LineBatch batch);

    internal class LineDrawStage : IRenderPipelineStage
    {
        public LineDrawStage(LineBatch lineBatch)
        {
            LineBatch = lineBatch;
        }

        public LineBatch LineBatch { get; }

        public event LineDrawAction Draw;

        public void Execute()
        {
            LineBatch.Clear();
            Draw?.Invoke(LineBatch);
            LineBatch.Draw();
        }
    }
}
