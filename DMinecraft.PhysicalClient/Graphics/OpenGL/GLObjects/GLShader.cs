using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects
{
    internal class GLShader : GLObject
    {
        private readonly int handle;

        public GLShader(ShaderType shaderType, GLContext context, string? name = null) : base(context, name)
        {
            handle = GL.CreateShader(shaderType);
            if (handle < 1)
            {
                throw new GLGraphicsException("Could not create shader.");
            }
            UpdateGLName();

            ShaderType = shaderType;
        }

        public int Handle => handle;

        public ShaderType ShaderType { get; }

        public string? InfoLog { get; private set; }

        protected override int NameHandle => Handle;

        protected override ObjectLabelIdentifier GLObjectLabelIdentifier => ObjectLabelIdentifier.Shader;

        public bool IsCompiled { get; private set; }

        protected override void DisposeInternal()
        {
            GL.DeleteShader(Handle);
        }

        public void Compile(string source)
        {
            GL.ShaderSource(Handle, source);

            GL.CompileShader(Handle);

            GL.GetShader(Handle, ShaderParameter.CompileStatus, out int compileStatus);
            InfoLog = GL.GetShaderInfoLog(Handle);
            IsCompiled = compileStatus != (int)All.NoError;
            if (!IsCompiled)
            {
                throw new GLGraphicsException($"Shader compilation error: {InfoLog}");
            }
        }
    }
}
