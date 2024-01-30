using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Textures.Atlas;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.UI
{
    internal class Sprite
    {
        public Vector3 Position { get; set; }

        public Vector2 Size { get; set; }

        public Vector3 Origin { get; set; }

        public Color4 Color { get; set; }

        //the super optimised system aims to use an array of texture2darray samplers
        //as it is estimated that the greatest cause for state changes is texture changes
        //program changes are indeed more costly (uniform updates are not),
        //so grouping by program is also a priority
        //the aim of the system is to batch sprites as efficiently as possible, to
        //minimise draw calls and state changes as well as minimising
        //cpu overhead by limiting the number of loops required
        //instead, a "baked" object providing access to dedicates sprite batches
        //for programs and textures is to be created at load time.

        public PackedTexture2DArrayAtlasItem TexCoords { get; set; }

        public int SamplerIndex { get; set; }
    }
}
