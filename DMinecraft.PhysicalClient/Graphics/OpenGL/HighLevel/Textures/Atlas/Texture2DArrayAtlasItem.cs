using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Textures.Atlas
{
    /// <summary>
    /// questionabel dispose smeantics, up for debate
    /// </summary>
    internal class Texture2DArrayAtlasItem : IDisposable
    {
        public Texture2DArrayAtlasItem(Texture2DArrayAtlas arrayAtlas, int layer)
        {
            ArrayAtlas = arrayAtlas;
            Layer = layer;
        }

        public int Layer { get; }

        public Texture2DArrayAtlas ArrayAtlas { get; }

        public void Dispose()
        {
            ArrayAtlas.FreeImage(this);
        }
    }
}
