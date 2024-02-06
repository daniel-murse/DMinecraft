using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites.Vertices
{
    internal interface ISpriteVerticesBatchConsumer
    {
        //how many left in the batch
        public int Remaining { get; }

        public bool IsFull { get; }

        public Span<SpriteVertices> SubmitSpritesAF(int spriteCount);

        public Span<SpriteVertices> SubmitSprites(int spriteCount);

        public void Flush();
    }
}
