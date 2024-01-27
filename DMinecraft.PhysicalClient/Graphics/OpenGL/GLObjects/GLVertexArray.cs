using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects.Data.VertexArrays;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects
{
    /// <summary>
    /// <see href="https://www.khronos.org/opengl/wiki/Vertex_Specification"/>
    /// </summary>
    internal class GLVertexArray : GLObject
    {
        private readonly int handle;

        public int Handle => handle;
        public GLVertexArray(GLContext context, string? name = null) : base(context, name)
        {
            GL.CreateVertexArrays(1, out handle);
            if (handle < 1)
            {
                throw new GLGraphicsException("Could not create VAO.");
            }
            UpdateGLName();

            vertexBufferBindings = new VertexBufferBinding[context.MaxVertexAttribBindings];
            vertexAttributeFormats = new VertexAttributeFormat[context.MaxVertexAttribs];
            vertexAttributeBindings = new int[context.MaxVertexAttribs];
            vertexAttributeEnableds = new bool[context.MaxVertexAttribs];
            vertexBufferBindingDivisors = new int[context.MaxVertexAttribBindings];
        }

        protected override int NameHandle => Handle;

        protected override ObjectLabelIdentifier GLObjectLabelIdentifier => ObjectLabelIdentifier.VertexArray;

        protected override void DisposeInternal()
        {
            GL.DeleteVertexArray(Handle);
        }

        private VertexBufferBinding?[] vertexBufferBindings;

        public VertexBufferBinding? GetVertexBufferBinding(int index)
        {
            EnsureIsVertexBufferBindingIndex(index);
            return vertexBufferBindings[index];
        }

        public void SetVertexBufferBinding(int index, VertexBufferBinding? vertexBufferBinding)
        {
            EnsureIsVertexBufferBindingIndex(index);
            if (vertexBufferBinding != null && vertexBufferBinding.Buffer != null)
            {
                GL.VertexArrayVertexBuffer(Handle, index, vertexBufferBinding.Buffer.Handle, vertexBufferBinding.OffsetBytes, vertexBufferBinding.StrideBytes);

            }
            else if (vertexBufferBinding == null)
            {
                GL.VertexArrayVertexBuffer(Handle, index, 0, 0, 0);
            }
            else
            {
                throw new ArgumentNullException();
            }
            vertexBufferBindings[index] = vertexBufferBinding;
        }

        private void EnsureIsVertexBufferBindingIndex(int index)
        {
            if (!(0 <= index && index < vertexBufferBindings.Length))
                throw new GLGraphicsException("Vertex buffer binding out of range.");
        }

        private VertexAttributeFormat?[] vertexAttributeFormats;

        public VertexAttributeFormat? GetVertexAttributeFormat(int index)
        {
            EnsureIsVertexAttributeIndex(index);
            return vertexAttributeFormats[index];
        }

        public void SetVertexAttributeFormat(int index, VertexAttributeFormat vertexAttribute)
        {
            EnsureIsVertexAttributeIndex(index);
            if (vertexAttribute.IsInt)
            {
                GL.VertexArrayAttribIFormat(Handle, index, vertexAttribute.Size, vertexAttribute.ComponentType, vertexAttribute.RelativeOffset);
            }
            else
            {
                GL.VertexArrayAttribFormat(Handle, index, vertexAttribute.Size, vertexAttribute.ComponentType, vertexAttribute.IsNormalised, vertexAttribute.RelativeOffset);
            }
            vertexAttributeFormats[index] = vertexAttribute;
        }

        private void EnsureIsVertexAttributeIndex(int attribute)
        {
            if(attribute < 0 || attribute > vertexAttributeFormats.Length)
            {
                throw new GLGraphicsException("Vertex attribute index out of range.");
            }
        }

        private int[] vertexAttributeBindings;

        public void SetVertexAttributeBinding(int attribute, int bufferBinding)
        {
            EnsureIsVertexAttributeIndex(attribute);
            EnsureIsVertexBufferBindingIndex(bufferBinding);
            GL.VertexArrayAttribBinding(Handle, attribute, bufferBinding);
            vertexAttributeBindings[attribute] = bufferBinding;
        }


        public int GetVertexAttributeBinding(int attribute) 
        {
            EnsureIsVertexAttributeIndex(attribute);
            return vertexAttributeBindings[attribute];
        }

        private bool[] vertexAttributeEnableds;

        public bool IsVertexAttributeEnabled(int attribute)
        {
            EnsureIsVertexAttributeIndex(attribute);
            return vertexAttributeEnableds[attribute];
        }

        public void SetVertexAttributeEnabled(int attribute, bool enabled)
        {
            EnsureIsVertexAttributeIndex(attribute);

            if (enabled)
                GL.EnableVertexArrayAttrib(Handle, attribute);
            else
                GL.DisableVertexArrayAttrib(Handle, attribute);

            vertexAttributeEnableds[attribute] = enabled;
        }

        private int[] vertexBufferBindingDivisors;

        public int GetVertexAttributeDivisor(int vertexBufferBinding)
        {
            EnsureIsVertexBufferBindingIndex(vertexBufferBinding);
            return vertexBufferBindingDivisors[vertexBufferBinding];
        }

        public void SetVertexBufferBindingDivisor(int vertexBufferBinding, int  divisor)
        {
            EnsureIsVertexBufferBindingIndex(vertexBufferBinding);
            GL.VertexArrayBindingDivisor(Handle, vertexBufferBinding, divisor);
            vertexBufferBindingDivisors[divisor] = vertexBufferBinding;
        }

        public void Use()
        {
            Context.UseVao(this);
        }

        private GLBuffer? indexBuffer;

        public GLBuffer? IndexBuffer
        {
            get
            {
                return indexBuffer;
            }
            set
            {
                if(value == null)
                {
                    GL.VertexArrayElementBuffer(Handle, 0);
                }
                else
                {
                    GL.VertexArrayElementBuffer(Handle, value.Handle);
                }
                indexBuffer = value;
            }
        }
    }
}
