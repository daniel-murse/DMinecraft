using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Text;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Util;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.UI.Widgets.Text
{
    internal class Label
    {
        public TextBuffer Text { get; set; }

        public Transform Transform { get; set; }

        public Color4 Color { get; set; }

        public Font Font { get; set; }

        public SpriteBatch SpriteBatch { get; set; }

        public SpriteRenderer SpriteRenderer { get; set; }

        //controls which sprite batch will be submitted to
        public int SpriteBatchChannel { get; set; }

        public void Render()
        {
            
        }
    }
}
