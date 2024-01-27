using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects
{
    internal class GLSampler : GLObject
    {
        private TextureWrapMode wrapR;
        private TextureWrapMode wrapS;
        private TextureWrapMode wrapT;
        private TextureMinFilter minFilter;
        private TextureMagFilter magFilter;
        private Vector4 borderColor;

        private readonly int handle;

        public GLSampler(GLContext context, string? name = null) : base(context, name)
        {
            GL.CreateSamplers(1, out handle);
            if (handle < 1)
                throw new GLGraphicsException("Could not create sampler.");
            UpdateGLName();

            SetDefaultState();
        }

        private void SetDefaultState()
        {
            //assumed default values are the same as textures
            //but need to check the gl wiki to make sure
            minFilter = TextureMinFilter.NearestMipmapLinear;
            magFilter = TextureMagFilter.Linear;

            wrapR = TextureWrapMode.Repeat;
            wrapS = TextureWrapMode.Repeat;
            wrapT = TextureWrapMode.Repeat;
        }

        public int Handle => handle;

        public TextureWrapMode WrapR
        {
            get => wrapR; private set
            {
                int casted = (int)wrapR;
                GL.TextureParameterI(Handle, TextureParameterName.TextureWrapR, ref casted);
                wrapR = value;
            }
        }

        public TextureWrapMode WrapS
        {
            get => wrapS; private set
            {
                int casted = (int)wrapS;
                GL.TextureParameterI(Handle, TextureParameterName.TextureWrapS, ref casted);
                wrapS = value;
            }
        }

        public TextureWrapMode WrapT
        {
            get => wrapT; private set
            {
                int casted = (int)wrapT;
                GL.TextureParameterI(Handle, TextureParameterName.TextureWrapT, ref casted);
                wrapT = value;
            }
        }

        public TextureMinFilter MinFilter
        {
            get => minFilter; private set
            {
                int casted = (int)minFilter;
                GL.TextureParameterI(Handle, TextureParameterName.TextureMinFilter, ref casted);
                minFilter = value;
            }
        }

        public TextureMagFilter MagFilter
        {
            get => magFilter; private set
            {
                int casted = (int)magFilter;
                GL.TextureParameterI(Handle, TextureParameterName.TextureMinFilter, ref casted);
                magFilter = value;
            }
        }

        public Vector4 BorderColor
        {
            get => borderColor; private set
            {
                GL.TextureParameter(Handle, TextureParameterName.TextureBorderColor, new float[] { value.X, value.Y, value.Z, value.W });
                borderColor = value;
            }
        }

        protected override int NameHandle => Handle;

        protected override ObjectLabelIdentifier GLObjectLabelIdentifier => ObjectLabelIdentifier.Sampler;

        protected override void DisposeInternal()
        {
            GL.DeleteSampler(Handle);
        }
    }
}
