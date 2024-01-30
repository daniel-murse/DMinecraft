using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites
{
    internal static class SpriteBatchExtensions
    {
        public static void Flush(this SpriteBatch spriteBatch) 
        {
            spriteBatch.Draw();
            spriteBatch.Clear();
        }

        public static Span<SpriteVertices> SubmitSpritesAF(this SpriteBatch spriteBatch, int sprites) 
        {
            if(spriteBatch.Remaining < sprites)
            {
                spriteBatch.Flush();
            }
            return spriteBatch.SubmitSprites(sprites);
        }

        public static void ChangeTexture2DArray(this SpriteBatch spriteBatch, GLTexture texture, int unit)
        {
            if(spriteBatch.Mesh.Vao.Context.GetTexture2DArray(unit) != texture)
            {
                spriteBatch.Flush();
            }
            spriteBatch.Mesh.Vao.Context.BindTexture2DArray(texture, unit);
        }
    }
}
