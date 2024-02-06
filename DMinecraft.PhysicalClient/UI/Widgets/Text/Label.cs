using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Lines;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites.Batches;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites.Optim;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites.Renderers;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Text;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Util;
using DMinecraft.PhysicalClient.UI.Widgets.Text.Alignment;
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

        public SpriteRenderBatch SpriteRenderBatch { get; set; }

        public LineRenderBatch LineRenderBatch { get; set; }

        public bool DoClipping { get; set; }

        public bool DoDebug { get; set; }

        public HorizontalTextAlignment HorizontalTextAlignment { get; set; }

        public Vector2 Size { get; set; }

        public void Render()
        {
            if (DoClipping)
            {

            }
            else
            {
                SpriteRenderBatch.UseTexture(Font.Texture);
                Text.Submit(Transform, SpriteRenderBatch, Color);
            }
            if (DoDebug)
            {
                LineRenderBatch.SubmitLinesAF(4).ComputeQuad(Text.RenderBounds.Xy, Text.RenderBounds.Zw, Color4.Blue, Transform);
            }
        }
    }
}
