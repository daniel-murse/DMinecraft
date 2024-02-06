using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites.Vertices;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Textures;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Util;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.UI.Widgets.Boxes.Bmp
{
    internal class Box : IWidget
    {
        public Box(BoxSprites boxSprites)
        {
            BoxSprites = boxSprites;
        }

        public BoxSprites BoxSprites { get; }
        public Vector4 Bounds { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }

        public Color4 Color { get; set; }

        public Transform Transform { get; set; }

        public void Render()
        {
            int xCount = (int)Math.Ceiling(Math.Min(Size.X - BoxSprites.DimensionSizePixels * 2, 0) / BoxSprites.DimensionSizePixels);
            int yCount = (int)Math.Ceiling(Math.Min(Size.Y - BoxSprites.DimensionSizePixels * 2, 0) / BoxSprites.DimensionSizePixels);

            var sb = BoxSprites.SpriteRenderBatch;


            //start bl, do corners
            var pos = new Vector3(Position);
            var tr = BoxSprites.Corner;

            sb.UseTexture(tr.Texture);

            sb.SubmitSpritesAF(1)[0].Compute(pos, Transform, BoxSprites.SizePixels, tr, Color);

            //br
            pos.X += BoxSprites.DimensionSizePixels * (xCount + 1);

            sb.SubmitSpritesAF(1)[0].Compute(pos, Transform, BoxSprites.SizePixels, tr.BottomRight.U, tr.BottomLeft.V, tr.BottomLeft.U, tr.TopLeft.V, tr.Layer, tr.Index, Color);

            //tr
            pos.Y += BoxSprites.DimensionSizePixels * (yCount + 1);

            sb.SubmitSpritesAF(1)[0].Compute(pos, Transform, BoxSprites.SizePixels, tr.BottomRight.U, tr.TopLeft.V, tr.BottomLeft.U, tr.BottomLeft.V, tr.Layer, tr.Index, Color);

            //tl
            pos.X = Position.X;

            sb.SubmitSpritesAF(1)[0].Compute(pos, Transform, BoxSprites.SizePixels, tr.BottomLeft.U, tr.TopLeft.V, tr.BottomRight.U, tr.BottomLeft.V, tr.Layer, tr.Index, Color);

            pos.Y -= BoxSprites.DimensionSizePixels;

            //do edges

            tr = BoxSprites.Edge;

            sb.UseTexture(tr.Texture);

            //left
            for (int i = 0; i < yCount; i++)
            {
                pos.Y -= BoxSprites.DimensionSizePixels;

                sb.SubmitSpritesAF(1)[0].Compute(pos, Transform, BoxSprites.SizePixels, tr, Color);
            }

            //right
            pos.X += (yCount + 1) * BoxSprites.DimensionSizePixels;

            for (int i = 0; i < yCount; i++)
            {
                var sprite = sb.SubmitSpritesAF(1);
                sprite[0].ComputePosition(pos, Transform, BoxSprites.SizePixels);
                sprite[0].ComputeColor(Color);

                sprite[0].ComputeIndexLayer(tr.Index, tr.Layer);

                sprite[0].ComputeUV180(tr);

                pos.Y += BoxSprites.DimensionSizePixels;
            }

            //top
            pos.Y -= BoxSprites.DimensionSizePixels;

            for (int i = 0; i < xCount; i++)
            {
                var sprite = sb.SubmitSpritesAF(1);

                sprite[0].ComputePosition(pos, Transform, BoxSprites.SizePixels);
                sprite[0].ComputeColor(Color);

                sprite[0].ComputeIndexLayer(tr.Index, tr.Layer);

                sprite[0].ComputeUV90(tr);

                pos.X += BoxSprites.DimensionSizePixels;
            }

            //bot

            pos.Y -= BoxSprites.DimensionSizePixels * (yCount + 1);

            for (int i = 0; i < xCount; i++)
            {
                pos.X -= BoxSprites.DimensionSizePixels;

                var sprite = sb.SubmitSpritesAF(1);

                sprite[0].ComputePosition(pos, Transform, BoxSprites.SizePixels);
                sprite[0].ComputeColor(Color);

                sprite[0].ComputeIndexLayer(tr.Index, tr.Layer);

                sprite[0].ComputeUV270(tr);
            }

            //now fill

            tr = BoxSprites.Fill;

            sb.UseTexture(tr.Texture);

            pos = new Vector3(Position) + new Vector3(BoxSprites.DimensionSizePixels, BoxSprites.DimensionSizePixels, 0);

            for (int i = 0; i < xCount; i++)
            {
                for (global::System.Int32 j = 0; j < yCount; j++)
                {
                    sb.SubmitSpritesAF(1)[0].Compute(pos, Transform, BoxSprites.SizePixels, tr, Color);
                    pos.Y += BoxSprites.DimensionSizePixels;
                }
                pos.Y -= BoxSprites.DimensionSizePixels * yCount;
                pos.X += BoxSprites.DimensionSizePixels;
            }
        }

        public void Update()
        {
        }
    }
}
