using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Shaders.Interfaces
{
    internal class ClipInterface
    {
        public const string DefaultUniformBlockName = "Clipping";

        public ClipInterface(GLProgram program, UniformBuffer<Vector4> clipPlaneBatch, string blockName = DefaultUniformBlockName)
        {
            Program = program;
            ClipPlanes = clipPlaneBatch;

            if (!Program.IsLinked)
                throw new ArgumentException();
            var block = Program.Interface.Uniform.UniformBlocks.Where(p => p.Name == blockName).SingleOrDefault();
            var member = (block ?? throw new ArgumentNullException()).Members.SingleOrDefault();



            Program.SetUniformBlockBinding(block.Index, ClipPlanes.BufferBinding);
        }

        public GLProgram Program { get; }

        public UniformBuffer<Vector4> ClipPlanes { get; }


    }
}
