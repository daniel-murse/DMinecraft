using OpenTK.Graphics.OpenGL4;
using StbImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects
{
    internal class GLTexture : GLObject
    {
        //https://registry.khronos.org/OpenGL-Refpages/gl4/html/glTexParameter.xhtml
        //https://registry.khronos.org/OpenGL-Refpages/gl4/html/glTexStorage2D.xhtml
        private readonly int handle;
        private TextureWrapMode wrapR;
        private TextureWrapMode wrapS;
        private TextureWrapMode wrapT;
        private TextureMinFilter minFilter;
        private TextureMagFilter magFilter;
        private Vector4 borderColor;
        private int[] swizzle;

        public int Handle => handle;

        public GLTexture(GLContext context, TextureTarget textureTarget, string? name = null) : base(context, name)
        {
            Target = textureTarget;
            GL.CreateTextures(textureTarget, 1, out handle);
            if (handle < 1)
            {
                throw new GLGraphicsException("Could not create texture.");
            }
            UpdateGLName();


            SetDefaultState();
        }

        private void SetDefaultState()
        {
            minFilter = TextureMinFilter.NearestMipmapLinear;
            magFilter = TextureMagFilter.Linear;
            swizzle = new int[] { (int)All.Red, (int)All.Green, (int)All.Blue, (int)All.Alpha };

            wrapR = TextureWrapMode.Repeat;
            wrapS = TextureWrapMode.Repeat;
            wrapT = TextureWrapMode.Repeat;
        }

        public TextureTarget Target { get; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public int Depth { get; private set; }

        public int MipLevels { get; private set; }

        public bool IsImmutable { get; private set; }

        public SizedInternalFormat InternalFormat { get; private set; }

        public TextureWrapMode WrapR
        {
            get => wrapR; set
            {
                int casted = (int)wrapR;
                GL.TextureParameter(Handle, TextureParameterName.TextureWrapR, casted);
                wrapR = value;
            }
        }

        public TextureWrapMode WrapS
        {
            get => wrapS; set
            {
                int casted = (int)wrapS;
                GL.TextureParameter(Handle, TextureParameterName.TextureWrapS, casted);
                wrapS = value;
            }
        }

        public TextureWrapMode WrapT
        {
            get => wrapT; set
            {
                int casted = (int)wrapT;
                GL.TextureParameter(Handle, TextureParameterName.TextureWrapT, casted);
                wrapT = value;
            }
        }

        public TextureMinFilter MinFilter
        {
            get => minFilter; set
            {
                int casted = (int)minFilter;
                GL.TextureParameter(Handle, TextureParameterName.TextureMinFilter, casted);
                minFilter = value;
            }
        }

        public TextureMagFilter MagFilter
        {
            get => magFilter; set
            {
                int casted = (int)magFilter;
                GL.TextureParameter(Handle, TextureParameterName.TextureMinFilter, casted);
                magFilter = value;
            }
        }

        public Vector4 BorderColor
        {
            get => borderColor; set
            {
                GL.TextureParameter(Handle, TextureParameterName.TextureBorderColor, new float[] { value.X, value.Y, value.Z, value.W });
                borderColor = value;
            }
        }

        public void Bind(int textureUnit)
        {
            GL.BindTextureUnit(Handle, textureUnit);
        }

        /// <summary>
        /// GL_RED, GL_GREEN, GL_BLUE, GL_ALPHA
        /// </summary>
        public All[] Swizzle
        {
            get
            {
                var arr = new All[swizzle.Length];
                Array.Copy(swizzle, arr, swizzle.Length);
                return arr;
            }

            set
            {
                if (value == null || value.Length != 4)
                    throw new ArgumentException();
                GL.TextureParameterI(Handle, TextureParameterName.TextureSwizzleRgba, swizzle);
                Array.Copy(value, swizzle, swizzle.Length);
            }
        }

        protected override int NameHandle => Handle;

        protected override ObjectLabelIdentifier GLObjectLabelIdentifier => ObjectLabelIdentifier.Texture;

        public void GenerateMipMaps()
        {
            GL.GenerateTextureMipmap(Handle);
        }

        public void CreateImmutable2D(int mips, int width, int height, SizedInternalFormat internalFormat)
        {
            Ensure2D();
            if (IsImmutable)
                throw new GLGraphicsException("Immutable texture cannot be modified.");
            GL.TextureStorage2D(Handle, mips, internalFormat, width, height);
            IsImmutable = true;
            Width = width;
            Height = height;
            Depth = 1;
            MipLevels = mips;
            InternalFormat = internalFormat;
        }

        private void Ensure2D()
        {
            if (Target != TextureTarget.Texture2D)
                throw new GLGraphicsException("2D function called on " + Target);
        }

        public void SubImage2D(int mip, int x, int y, int w, int h, PixelFormat pixelFormat, PixelType pixelType, Span<byte> data)
        {
            Ensure2D();
            if (x < 0 || y < 0 || w + x > Width || y + h > Height)
                throw new ArgumentException("Invalid texture rect.");
            GL.TextureSubImage2D(Handle, mip, x, y, w, h, pixelFormat, pixelType, ref data[0]);
        }

        protected override void DisposeInternal()
        {
            GL.DeleteTexture(handle);
        }

        /// <summary>
        /// Also used for 2D arrays.
        /// </summary>
        /// <param name="mips"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="depth"></param>
        /// <param name="internalFormat"></param>
        /// <exception cref="GLGraphicsException"></exception>
        internal void CreateImmutable3D(int mips, int width, int height, int depth, SizedInternalFormat internalFormat)
        {
            if (IsImmutable)
                throw new GLGraphicsException("Immutable texture cannot be modified.");
            GL.TextureStorage3D(Handle, mips, internalFormat, width, height, depth);
            IsImmutable = true;
            Width = width;
            Height = height;
            Depth = depth;
            MipLevels = mips;
            InternalFormat = internalFormat;
        }

        /// <summary>
        /// Also used for 2D arrays.
        /// </summary>
        /// <param name="mip"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="d"></param>
        /// <param name="pixelFormat"></param>
        /// <param name="pixelType"></param>
        /// <param name="data"></param>
        /// <exception cref="ArgumentException"></exception>
        public void SubImage3D(int mip, int x, int y, int z, int w, int h, int d, PixelFormat pixelFormat, PixelType pixelType, Span<byte> data)
        {
            if (x < 0 || y < 0 || w + x > Width || y + h > Height)
                throw new ArgumentException("Invalid texture rect.");
            GL.TextureSubImage3D(Handle, mip, x, y, z, w, h, d, pixelFormat, pixelType, ref data[0]);
        }
    }
}
