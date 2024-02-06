using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites.Batches;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites.Vertices;
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
        private MeasureResult measurement;

        public TextBuffer()
        {
            HbBuffer = new HarfBuzzSharp.Buffer();
        }

        public HarfBuzzSharp.Buffer HbBuffer { get; private set; }

        public Font? Font { get; private set; }

        public TextDirection TextDirection { get; private set; }

        //measures pixel bounds
        public Vector4 RenderBounds => measurement.RenderBounds;

        public Vector2 RenderSize => measurement.RenderSize;

        //measures in terms of cursor, line height
        //static in terms of line size
        public Vector4 FormatBounds => measurement.FormatBounds;

        public Vector2 FormatSize => measurement.FormatSize;

        public MeasureResult Measurement { get => measurement; private set => measurement = value; }

        public void SetText(Font font, string text, TextDirection textDirection = TextDirection.LTR)
        {
            Font = font;

            HbBuffer.ClearContents();
            HbBuffer.AddUtf16(text);

            HbBuffer.GuessSegmentProperties();
            HbBuffer.Direction = DirToHbDir(textDirection);

            Font.HbFont.Shape(HbBuffer, null);

            if (TextDirection == TextDirection.LTR || TextDirection == TextDirection.RTL)
            {
                Font.MeasureTextHori(HbBuffer, out measurement);
            }
            else
            {
                Font.MeasureTextVert(HbBuffer, out measurement);
            }
        }

        private HarfBuzzSharp.Direction DirToHbDir(TextDirection dir) 
        {
            return dir switch
            {
                TextDirection.LTR => HarfBuzzSharp.Direction.LeftToRight,
                TextDirection.RTL => HarfBuzzSharp.Direction.RightToLeft,
                TextDirection.TTB => HarfBuzzSharp.Direction.TopToBottom,
                TextDirection.BTT => HarfBuzzSharp.Direction.BottomToTop,
                _ => HarfBuzzSharp.Direction.Invalid
            };
        }

        public void Submit(Transform transform, ISpriteVerticesBatchConsumer spriteBatch, Color4 color)
        {
            Font?.SubmitText(HbBuffer, spriteBatch, transform, color);
        }

        public void Dispose()
        {
            HbBuffer.Dispose();
        }

        public Vector3 RenderCenterOrigin => new Vector3((RenderSize / 2) + RenderBounds.Xy);

        public Vector3 FormatCenterOrigin => new Vector3((FormatSize / 2) + FormatBounds.Xy);
    }
}
