using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Pipeline
{
    internal class ClearFramebufferStage : IRenderPipelineStage
    {
        public ClearBufferMask ClearBufferMask { get; set; }

        public void Execute()
        {
            GL.Clear(ClearBufferMask);
        }
    }
}
