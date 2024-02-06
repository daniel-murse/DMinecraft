using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Text;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Textures;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Util;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites.Vertices
{
    internal static class SpriteVerticesExtensions
    {
        public static void Compute(ref this SpriteVertices sprite, Vector3 position, Transform transform, Vector2 size, ushort texLeft, ushort texBot, ushort texRight, ushort texTop, int layer, int index, Color4 color)
        {
            ComputePosition(ref sprite, position, transform, size);
            ComputeColor(ref sprite, color);
            ComputeUV(ref sprite, texLeft, texBot, texRight, texTop);

            ComputeIndexLayer(ref sprite, layer, index);

        }

        public static void ComputeUV(ref this SpriteVertices sprite, ushort texLeft, ushort texBot, ushort texRight, ushort texTop)
        {
            sprite.BottomLeft.U = texLeft;
            sprite.BottomLeft.V = texBot;
            sprite.BottomRight.U = texRight;
            sprite.BottomRight.V = texBot;
            sprite.TopLeft.U = texLeft;
            sprite.TopLeft.V = texTop;
            sprite.TopRight.U = texRight;
            sprite.TopRight.V = texTop;
        }

        public static void ComputeColor(this ref SpriteVertices sprite, Color4 color)
        {
            sprite.BottomLeft.Color = color.ToRgba();
            sprite.BottomRight.Color = color.ToRgba();
            sprite.TopLeft.Color = color.ToRgba();
            sprite.TopRight.Color = color.ToRgba();
        }

        public static void ComputePosition(this ref SpriteVertices sprite, Vector3 position, Transform transform, Vector2 size)
        {
            //Vector3.TransformVector ignores the bottom row (translation) in this case :/
            position -= transform.Origin;

            Vector3.TransformVector(position, in transform.RefMatrix, out sprite.BottomLeft.Position);
            sprite.BottomLeft.Position += transform.Position;

            position.X += size.X;
            Vector3.TransformVector(position, in transform.RefMatrix, out sprite.BottomRight.Position);
            sprite.BottomRight.Position += transform.Position;

            position.Y += size.Y;
            Vector3.TransformVector(position, in transform.RefMatrix, out sprite.TopRight.Position);
            sprite.TopLeft.Position += transform.Position;

            position.X -= size.X;
            Vector3.TransformVector(position, in transform.RefMatrix, out sprite.TopLeft.Position);
            sprite.TopRight.Position += transform.Position;
        }

        public static void Compute(ref this SpriteVertices sprite, Vector3 position, Transform transform, Vector2 size,TextureRegion textureRegion, Color4 color)
        {
            ComputePosition(ref sprite, position, transform, size);
            ComputeColor(ref sprite, color);
            ComputeUV(ref sprite, textureRegion);
            ComputeIndexLayer(ref sprite, textureRegion.Layer, textureRegion.Index);

        }

        public static void ComputeUV(ref this SpriteVertices sprite, TextureRegion textureRegion)
        {
            sprite.BottomLeft.U = textureRegion.BottomLeft.U;
            sprite.BottomLeft.V = textureRegion.BottomLeft.V;
            sprite.BottomRight.U = textureRegion.BottomRight.U;
            sprite.BottomRight.V = textureRegion.BottomRight.V;
            sprite.TopLeft.U = textureRegion.TopLeft.U;
            sprite.TopLeft.V = textureRegion.TopLeft.V;
            sprite.TopRight.U = textureRegion.TopRight.U;
            sprite.TopRight.V = textureRegion.TopRight.V;
        }

        public static void ComputeIndexLayer(this ref SpriteVertices sprite, int layer, int index)
        {
            sprite.BottomLeft.Layer = layer;
            sprite.TopLeft.Layer = layer;
            sprite.BottomRight.Layer = layer;
            sprite.TopRight.Layer = layer;

            sprite.BottomLeft.Index = index;
            sprite.BottomRight.Index = index;
            sprite.TopLeft.Index = index;
            sprite.TopRight.Index = index;
        }

        public static void RotateUV90(ref this SpriteVertices sprite)
        {
            sprite.BottomLeft.U = sprite.BottomRight.U;
            sprite.BottomLeft.V = sprite.BottomRight.V;
            sprite.BottomRight.U = sprite.TopRight.U;
            sprite.BottomRight.V = sprite.TopRight.V;
            sprite.TopLeft.U = sprite.BottomLeft.U;
            sprite.TopLeft.V = sprite.BottomLeft.V;
            sprite.TopRight.U = sprite.TopLeft.U;
            sprite.TopRight.V = sprite.TopLeft.V;
        }

        public static void ComputeUV90(ref this SpriteVertices sprite, TextureRegion textureRegion)
        {
            //could be optimised but whatever
            sprite.ComputeUV(textureRegion);
            sprite.RotateUV90();
        }

        public static void ComputeUV180(ref this SpriteVertices sprite, TextureRegion textureRegion)
        {
            //could be optimised but whatever
            sprite.ComputeUV(textureRegion);
            sprite.RotateUV90();
            sprite.RotateUV90();
        }

        public static void ComputeUV270(ref this SpriteVertices sprite, TextureRegion textureRegion)
        {
            sprite.ComputeUV(textureRegion);
            sprite.RotateUV90();
            sprite.RotateUV90();
            sprite.RotateUV90();
        }
    }
}
