using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects.Data.Programs;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects
{
    internal class GLProgram : GLObject
    {
        private readonly int handle;

        public GLProgram(GLContext context, string? name = null) : base(context, name)
        {
            handle = GL.CreateProgram();
            
            if(handle < 1)
            {
                throw new GLGraphicsException("Could not create program.");
            }
            UpdateGLName();

        }

        public int Handle { get { return handle; } }

        protected override int NameHandle => Handle;

        protected override ObjectLabelIdentifier GLObjectLabelIdentifier => ObjectLabelIdentifier.Program;

        protected override void DisposeInternal()
        {
            GL.DeleteProgram(Handle);
        }
        [MemberNotNullWhen(true, nameof(programInterface))]
        public bool IsLinked { get; private set; }
        public string InfoLog { get; private set; }
        public LinkedProgramInterface Interface { get => !IsLinked ? throw new GLGraphicsException() : programInterface; }

        public void Link(params GLShader[] shaders)
        {
            foreach (var shader in shaders)
            {
                GL.AttachShader(Handle, shader.Handle);
            }

            GL.LinkProgram(Handle);

            foreach (var shader in shaders)
            {
                GL.DetachShader(Handle, shader.Handle);
            }

            GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out int linkStatus);

            IsLinked = (int)All.NoError != linkStatus;
            InfoLog = GL.GetProgramInfoLog(Handle);

            if(!IsLinked)
            {
                programInterface = null;
                throw new GLGraphicsException("Program failed linking. " + InfoLog);
            }

            programInterface = RetrieveLinkedProgramInterface();

            uniformBlockBindings = new int[Interface.Uniform.UniformBlocks.Count];
        }

        private LinkedProgramInterface RetrieveLinkedProgramInterface()
        {
            //https://registry.khronos.org/OpenGL-Refpages/gl4/html/glGetProgramInterface.xhtml
            //https://registry.khronos.org/OpenGL-Refpages/gl4/html/glGetProgramResource.xhtml
            //secondary, just for clarity on what they do
            //https://registry.khronos.org/OpenGL-Refpages/gl4/html/glGetProgramResourceIndex.xhtml
            //https://registry.khronos.org/OpenGL-Refpages/gl4/html/glGetProgramResourceLocation.xhtml

            return new LinkedProgramInterface()
            {
                Input = RetrieveInputInterface(),
                Output = RetrieveOutputInterface(),
                Uniform = RetrieveUniformInterface(),
                Subroutine = RetrieveSubroutineInterface()
            };
        }

        private SubroutineInterface RetrieveSubroutineInterface()
        {
            //todo subroutines
            return new SubroutineInterface();
        }

        private UniformInterface RetrieveUniformInterface()
        {
            GL.GetProgramInterface(Handle, ProgramInterface.UniformBlock, ProgramInterfaceParameter.ActiveResources, out int uniformBlockCount);

            var uniformBlockQuery = new ProgramProperty[]
            {
                ProgramProperty.BufferDataSize,
                ProgramProperty.NumActiveVariables,
                ProgramProperty.NameLength
            };

            var uniformBlockResult = new int[uniformBlockQuery.Length];

            var uniformBlockMembersArrays = new List<UniformBlockMember>[uniformBlockCount];

            var uniformBlocks = new UniformBlock[uniformBlockCount];

            for (int i = 0; i < uniformBlockCount; i++)
            {
                GL.GetProgramResource(Handle, ProgramInterface.UniformBlock, i, uniformBlockQuery.Length, uniformBlockQuery, uniformBlockResult.Length, out int uniformBlockResultLength, uniformBlockResult);
                GL.GetProgramResourceName(Handle, ProgramInterface.UniformBlock, i, uniformBlockResult[2], out int nameLength, out string name);
                uniformBlockMembersArrays[i] = new List<UniformBlockMember>(uniformBlockResult[1]);
                uniformBlocks[i] = new UniformBlock()
                {
                    Members = uniformBlockMembersArrays[i],
                    SizeBytes = uniformBlockResult[0],
                    Name = name,
                    Index = i
                };
            }

            GL.GetProgramInterface(Handle, ProgramInterface.Uniform, ProgramInterfaceParameter.ActiveResources, out int uniformCount);

            var uniformQuery = new ProgramProperty[]
            {
                ProgramProperty.Location,
                ProgramProperty.Type,
                ProgramProperty.ArraySize,
                ProgramProperty.NameLength,
                ProgramProperty.BlockIndex,
                ProgramProperty.Offset,
                ProgramProperty.ArrayStride
            };

            var uniformResult = new int[uniformQuery.Length];

            var uniformResources = new List<UniformResource>(uniformCount);

            for (int i = 0; i < uniformCount; i++)
            {
                GL.GetProgramResource(Handle, ProgramInterface.Uniform, i, uniformQuery.Length, uniformQuery, uniformResult.Length, out int uniformResultLength, uniformResult);

                GL.GetProgramResourceName(Handle, ProgramInterface.Uniform, i, uniformResult[3], out int nameLength, out string name);

                if (uniformResult[0] != -1)
                {
                    uniformResources.Add(new UniformResource()
                    {
                        Location = uniformResult[0],
                        Type = uniformResult[1],
                        ArraySize = uniformResult[2],
                        Name = name
                    });
                }
                else
                {
                    uniformBlockMembersArrays[uniformResult[4]].Add(new UniformBlockMember()
                    {
                        Type = uniformResult[1],
                        ArraySize = uniformResult[2],
                        OffsetBytes = uniformResult[5],
                        ArrayStrideBytes = uniformResult[6],
                        Name = name
                    });
                }
            }

            return new UniformInterface() { Uniforms = uniformResources, UniformBlocks = uniformBlocks };
        }

        private InputInterface RetrieveInputInterface()
        {
            GL.GetProgramInterface(Handle, ProgramInterface.ProgramInput, ProgramInterfaceParameter.ActiveResources, out int attributeCount);

            ProgramProperty[] query = new ProgramProperty[]
            {
                ProgramProperty.Location,
                ProgramProperty.Type,
                ProgramProperty.ArraySize,
                ProgramProperty.NameLength
            };

            InputResource[] inputResources = new InputResource[attributeCount];

            int[] result = new int[query.Length];

            for (int i = 0; i < attributeCount; i++)
            {
                GL.GetProgramResource(Handle, ProgramInterface.ProgramInput, i, query.Length, query, result.Length, out int resultCount, result);
                if(resultCount != result.Length)
                {
                    throw new GLGraphicsException("Failed to get program resource.");
                }

                GL.GetProgramResourceName(Handle, ProgramInterface.ProgramInput, i, result[3], out int nameLength, out string name);

                var inputResource = new InputResource(result[0], result[1], result[2], name);

                inputResources[i] = inputResource;
            }

            return new InputInterface() { Resources = inputResources };
        }

        private OutputInterface RetrieveOutputInterface()
        {
            GL.GetProgramInterface(Handle, ProgramInterface.ProgramInput, ProgramInterfaceParameter.ActiveResources, out int attributeCount);

            ProgramProperty[] query = new ProgramProperty[]
            {
                ProgramProperty.Location,
                ProgramProperty.Type,
                ProgramProperty.ArraySize,
                ProgramProperty.NameLength
            };

            OutputResource[] inputResources = new OutputResource[attributeCount];

            int[] result = new int[query.Length];

            for (int i = 0; i < attributeCount; i++)
            {
                GL.GetProgramResource(Handle, ProgramInterface.ProgramInput, i, query.Length, query, result.Length, out int resultCount, result);
                if (resultCount != result.Length)
                {
                    throw new GLGraphicsException("Failed to get program resource.");
                }

                GL.GetProgramResourceName(Handle, ProgramInterface.ProgramInput, i, result[3], out int nameLength, out string name);

                var inputResource = new OutputResource(result[0], result[1], result[2], name);

                inputResources[i] = inputResource;
            }

            return new OutputInterface() { Resources = inputResources };
        }

        private int[] uniformBlockBindings;
        private LinkedProgramInterface? programInterface;

        public void SetUniformBlockBinding(int blockIndex, int bufferBinding)
        {
            GL.UniformBlockBinding(Handle, blockIndex, bufferBinding);
        }

        public int GetUniformBlockBinding(int blockIndex)
        {
            return uniformBlockBindings[blockIndex];
        }
    }
}
