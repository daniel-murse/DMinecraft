using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites.Optim
{
    internal class SpriteDrawStageBuilder
    {
        List<SpriteDrawStageResource> resources;

        public int AddRegion(SpriteRenderer renderer, SpriteBatch spritebatch) 
        {
            var rs = new SpriteDrawStageResource() { Renderer = renderer, Batch = spritebatch };
            resources.Add(rs);
            return resources.Count - 1;
        }
    }
}
