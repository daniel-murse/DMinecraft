using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Text;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Util;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites
{
    internal static class SpriteVerticesExtensions
    {
        public static void Compute(ref this SpriteVertices sprite, Vector3 position, Transform transform, Vector2 size, ushort texLeft, ushort texBot, ushort texRight, ushort texTop, int layer, int index, Color4 color)
        {
            /*
             * We have choices:
             * A matrix 4
             */

            Vector3.TransformVector(position, in transform.RefMatrix, out sprite.BottomLeft.Position);

            position.X += size.X;
            Vector3.TransformVector(position, in transform.RefMatrix, out sprite.BottomRight.Position);

            position.Y += size.Y;
            Vector3.TransformVector(position, in transform.RefMatrix, out sprite.TopRight.Position);

            position.X -= size.X;
            Vector3.TransformVector(position, in transform.RefMatrix, out sprite.TopLeft.Position);


            sprite.BottomLeft.Index = layer;
            sprite.BottomRight.Index = layer;
            sprite.TopLeft.Index = layer;
            sprite.TopRight.Index = layer;

            sprite.BottomLeft.Color = color.ToRgba();
            sprite.BottomRight.Color = color.ToRgba();
            sprite.TopLeft.Color = color.ToRgba();
            sprite.TopRight.Color = color.ToRgba();

            sprite.BottomLeft.U = texLeft;
            sprite.BottomLeft.V = texBot;
            sprite.BottomRight.U = texRight;
            sprite.BottomRight.V = texBot;
            sprite.TopLeft.U = texLeft;
            sprite.TopLeft.V = texTop;
            sprite.TopRight.U = texRight;
            sprite.TopRight.V = texTop;

            sprite.BottomLeft.Layer = layer;
            sprite.TopLeft.Layer = layer;
            sprite.BottomRight.Layer = layer;
            sprite.TopRight.Layer = layer;

            sprite.BottomLeft.Index = index;
            sprite.BottomRight.Index = index;
            sprite.TopLeft.Index = index;
            sprite.TopRight.Index = index;

        }
    }
}
