using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Text.Content
{
    internal interface IFontSource
    {
        public HarfBuzzSharp.Face Face { get; }

        
    }
}
