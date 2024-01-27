using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Pipeline
{
    /// <summary>
    /// 
    /// </summary>
    internal class RenderPipeline
    {
        public RenderPipeline()
        {
            Stages = new List<IRenderPipelineStage>();
        }

        public IList<IRenderPipelineStage> Stages { get; }

        public void Render(TimeSpan deltaTime)
        {
            foreach (var item in Stages)
            {
                item.Execute();
            }
        }
    }
}
