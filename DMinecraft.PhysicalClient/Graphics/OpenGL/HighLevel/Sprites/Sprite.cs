using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Textures.Atlas;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Util;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites
{
    internal class Sprite
    {
        public Vector3 Position { get; set; }

        public Vector3 Size { get; set; }

        public Vector3 Origin { get; set; }

        public Vector3 Scale {  get; set; }

        public Color4 Color { get; set; }

        public PackedTexture2DArrayAtlasItem Texture { get; set; }

        public void ComputeVertices(ref SpriteVertices vertices)
        {
            vertices.BottomLeft.Position = Position;

            vertices.BottomRight.Position = Position;
            vertices.BottomRight.Position.X += Size.X;

            vertices.TopLeft.Position = Position;
            vertices.TopLeft.Position.Y += Size.Y;

            vertices.TopRight.Position = Position;
            vertices.TopRight.Position.X += Size.X;
            vertices.TopRight.Position.Y += Size.Y;

            vertices.BottomLeft.Color = Color.ToRgba();
            vertices.BottomRight.Color = Color.ToRgba();
            vertices.TopLeft.Color = Color.ToRgba();
            vertices.TopRight.Color = Color.ToRgba();

            vertices.BottomLeft.Layer = Texture.Layer;
            vertices.TopLeft.Layer = Texture.Layer;
            vertices.BottomRight.Layer = Texture.Layer;
            vertices.TopRight.Layer = Texture.Layer;

            vertices.BottomLeft.U = Texture.Left;
            vertices.BottomLeft.V = Texture.Bottom;
            vertices.TopLeft.U = Texture.Left;
            vertices.TopLeft.V = Texture.Top;
            vertices.TopRight.U = Texture.Right;
            vertices.TopRight.V = Texture.Top;
            vertices.BottomRight.U = Texture.Right;
            vertices.BottomRight.V = Texture.Bottom;
        }
    }
}
