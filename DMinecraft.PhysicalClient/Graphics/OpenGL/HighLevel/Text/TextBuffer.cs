using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Util;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Text
{
    internal class TextBuffer : IDisposable
    {
        private Vector4 bounds;
        private Vector2 boundsSize;

        public TextBuffer()
        {
            HbBuffer = new HarfBuzzSharp.Buffer();
        }

        public HarfBuzzSharp.Buffer HbBuffer { get; set; }

        public Font? Font { get; set; }

        public Vector4 Bounds { get => bounds; set => bounds = value; }

        public Vector2 BoundsSize { get => boundsSize; set => boundsSize = value; }

        public void SetText(Font font, string text)
        {
            HbBuffer.ClearContents();
            HbBuffer.AddUtf16(text);
            HbBuffer.GuessSegmentProperties();
            Font = font;
            Font.HbFont.Shape(HbBuffer, null);
            Font.MeasureText(HbBuffer, out bounds, out boundsSize);
        }

        public void Submit(Transform transform, SpriteBatch spriteBatch, Color4 color)
        {
            Font?.SubmitText(HbBuffer, spriteBatch, transform, color);
        }

        public void Dispose()
        {
            HbBuffer.Dispose();
        }
    }
}
