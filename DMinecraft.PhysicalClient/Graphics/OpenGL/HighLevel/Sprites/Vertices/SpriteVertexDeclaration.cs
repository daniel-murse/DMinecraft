﻿using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects.Data.Programs;
using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites.Vertices
{
    //representation of shader program input attributes
    internal class SpriteVertexDeclaration
    {
        public required int PositionLocation { get; init; }
        public required int ColorLocation { get; init; }
        public required int UVLocation { get; init; }
        public required int LayerIndexLocation { get; init; }


        [SetsRequiredMembers]
        public SpriteVertexDeclaration(InputInterface input)
        {
            PositionLocation = input.Resources.Where(p => p.Location == 0 && p.Type == (int)All.FloatVec3 && p.ArraySize == 1).FirstOrDefault()?.Location ?? throw new GLGraphicsException("Position not found at location 0.");
            ColorLocation = input.Resources.Where(p => p.Location == 1 && p.Type == (int)All.UnsignedInt && p.ArraySize == 1).FirstOrDefault()?.Location ?? throw new GLGraphicsException("Color not found at location 1.");
            UVLocation = input.Resources.Where(p => p.Location == 2 && p.Type == (int)All.FloatVec2 && p.ArraySize == 1).FirstOrDefault()?.Location ?? throw new GLGraphicsException("UV not found at location 2.");
            LayerIndexLocation = input.Resources.Where(p => p.Location == 3 && p.Type == (int)All.IntVec2 && p.ArraySize == 1).FirstOrDefault()?.Location ?? throw new GLGraphicsException("Layer & index not found at location 3.");
        }
    }
}
