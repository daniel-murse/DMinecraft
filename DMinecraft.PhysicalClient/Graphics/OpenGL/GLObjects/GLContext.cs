using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects.Data.Buffers;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects
{
    internal class GLContext
    {
        #region VAOs
        /// <summary>
        /// The number of buffer binding points for vertex arrays.
        /// </summary>
        public int MaxVertexAttribBindings { get; private set; }

        /// <summary>
        /// The number of vertex attributes available in shaders and for vaos.
        /// </summary>
        public int MaxVertexAttribs { get; private set; }

        /// <summary>
        /// Should be less than 2048. The maximum stride between vertex attributes in buffer binding points.
        /// Limit relevant to <see cref="GL.VertexArrayVertexBuffer(int, int, int, nint, int)"/>
        /// </summary>
        public int MaxVertexAttribStrideBytes { get; private set; }

        /// <summary>
        /// At least 2047. The maximum relative offset from where vertex attributes start 
        /// in their buffer binding. Limit relevant to
        /// <see cref="GL.VertexArrayAttribFormat(int, int, int, VertexAttribType, bool, int)"/>
        /// </summary>
        public int MaxVertexAttribRelativeOffsetBytes { get; private set; }

        public GLVertexArray? Vao { get; private set; }
        #endregion

        #region Framebuffers

        /// <summary>
        /// Maximum number of outputs (color attachments) a shader can write to in a framebuffer.
        /// At least 8.
        /// </summary>
        public int MaxDrawBuffers { get; private set; }
        /// <summary>
        /// Maximum number of color attachments a shader
        /// </summary>
        public int MaxColorAttachments { get; private set; }

        #endregion

        #region Programs & Pipelines

        public GLProgram Program { get; private set; }

        #endregion

        #region Buffers

        public int MaxUniformBufferBindings { get; private set; }

        private BufferRange[] uniformBufferBindings;

        #endregion

        #region Textures

        public int MaxTextureUnits { get; private set; }

        public int MaxTextureDimensionSize { get; private set; }

        public int MaxTextureArrayLayers { get; private set; }

        private GLTexture?[] texture2DArrayUnits;

        #endregion

        #region Pixel transfer

        public int PixelUnpackAlignment { get; private set; }

        #endregion

        public string? Name { get; }

        public GLContext(string? name = null)
        {
            LoadConstants();
            Name = name;
            uniformBufferBindings = new BufferRange[MaxUniformBufferBindings];
            texture2DArrayUnits = new GLTexture[MaxTextureUnits];
            freeTextureUnits = new Queue<int>(Enumerable.Range(0, MaxTextureUnits));
        }

        private void LoadConstants()
        {
            MaxVertexAttribBindings = GL.GetInteger((GetPName)All.MaxVertexAttribBindings);
            MaxVertexAttribs = GL.GetInteger(GetPName.MaxVertexAttribs);
            MaxVertexAttribStrideBytes = GL.GetInteger((GetPName)All.MaxVertexAttribStride);
            MaxVertexAttribRelativeOffsetBytes = GL.GetInteger((GetPName)All.MaxVertexAttribRelativeOffset);

            MaxDrawBuffers = GL.GetInteger(GetPName.MaxDrawBuffers);
            MaxColorAttachments = GL.GetInteger(GetPName.MaxColorAttachments);

            MaxUniformBufferBindings = GL.GetInteger(GetPName.MaxUniformBufferBindings);

            MaxTextureUnits =  GL.GetInteger(GetPName.MaxCombinedTextureImageUnits);
            MaxTextureDimensionSize = GL.GetInteger(GetPName.MaxTextureSize);
            MaxTextureArrayLayers = GL.GetInteger(GetPName.MaxArrayTextureLayers);

            PixelUnpackAlignment = GL.GetInteger(GetPName.UnpackAlignment);
        }

        
        public void EnableDebug()
        {

        }

        private void OnDebug(DebugSource source, DebugType type, int id, DebugSeverity severity, int length, IntPtr message, IntPtr userParam)
        {

        }

        public void UseVao(GLVertexArray? vao)
        {
            if(Vao != vao)
            {
                Vao = vao;
                GL.BindVertexArray(vao?.Handle ?? 0);
            }
        }

        public void UseProgram(GLProgram program)
        {
            if (Program != program)
            {
                Program = program;
                GL.UseProgram(Program?.Handle ?? 0);
            }
        }

        public void BindUniformBuffer(GLBuffer? buffer, int bindingIndex, int offsetBytes, int sizeBytes) 
        {
            uniformBufferBindings[bindingIndex] = new BufferRange() { Buffer = buffer, OffsetBytes = offsetBytes, SizeBytes = sizeBytes };
            GL.BindBufferRange(BufferRangeTarget.UniformBuffer, bindingIndex, buffer?.Handle ?? 0, offsetBytes, sizeBytes);
        }

        public int BindUniformBuffer(GLBuffer buffer, int offsetBytes, int sizeBytes)
        {
            //could re implement this with a queue approach where all free things are immediately
            //in the queue
            //but this isnt expected to be a performance hit so whatever for nwo
            int i = 0;
            for (; i < uniformBufferBindings.Length; i++)
            {
                if (uniformBufferBindings[i] == null)
                    break;
            }
            if (i == uniformBufferBindings.Length)
                throw new GLGraphicsException("No free uniform buffer binding.");
            BindUniformBuffer(buffer, i, offsetBytes, sizeBytes);
            
            return i;
        }

        public void UnbindUniformBuffer(int bindingIndex)
        {
            BindUniformBuffer(null, bindingIndex, 0, 0);
        }

        public void BindTexture2DArray(GLTexture texture, int unit)
        {
            if (texture2DArrayUnits[unit] != texture)
            {
                GL.BindTextureUnit(unit, texture?.Handle ?? 0);
                texture2DArrayUnits[unit] = texture;
            }
        }

        public GLTexture? GetTexture2DArray(int unit)
        {
            return texture2DArrayUnits[unit];
        }

        private Queue<int> freeTextureUnits;

        public int ReserveTextureUnit()
        {
            if(freeTextureUnits.TryDequeue(out var unit))
            {
                return unit;
            }
            throw new GLGraphicsException("No free texture unit.");
        }

        public void FreeTextureUnit(int unit)
        {
            freeTextureUnits.Enqueue(unit);
        }

        public void SetUnpackAlignment(int unpackAlignment)
        {
            GL.PixelStore(PixelStoreParameter.UnpackAlignment, unpackAlignment);
            PixelUnpackAlignment = unpackAlignment;
        }
    }
}
