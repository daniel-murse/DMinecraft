using DMinecraft.PhysicalClient.Content;
using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Shaders.Content
{
    internal static class ShaderCacheExtensions
    {
        public static GLShader GetVertexShader(this ShaderCache shaderCache, string name)
        {
            return shaderCache.GetShader(name + ".vs", ShaderType.VertexShader);
        }

        public static GLShader GetFragmentShader(this ShaderCache shaderCache, string name)
        {
            return shaderCache.GetShader(name + ".fs", ShaderType.FragmentShader);
        }

        public static void LoadShaderFromFile(this ShaderCache shaderCache, string filePath)
        {
            var shader = new GLShader(GetShaderTypeFromExtension(filePath), shaderCache.Context);
            try
            {
                shader.Compile(File.ReadAllText(filePath));
            }
            catch (Exception)
            {
                shader.Dispose();
                throw;
            }
            shaderCache.AddItem(filePath, shader);
        }

        private static ShaderType GetShaderTypeFromExtension(string filePath)
        {
            return Path.GetExtension(filePath) switch
            {
                ".vs" => ShaderType.VertexShader,
                ".fs" => ShaderType.FragmentShader,
                ".gs" => ShaderType.GeometryShader,
                ".tcs" => ShaderType.TessControlShader,
                ".tes" => ShaderType.TessEvaluationShader,
                _ => throw new GLGraphicsException("Shader extension could not be inferred.")
            };
        }

        public static GLShader GetShader(this ShaderCache shaderCache, string name, ShaderType shaderType)
        {
            var shader = shaderCache.GetItem(name);
            if (shader.Item.ShaderType != shaderType)
                throw new GLGraphicsException("Incorrect shader type.");
            return shader.Item;
        }

        public static ShaderCacheCoroutine CreateCoroutine(this ShaderCache shaderCache, string path)
        {
            return new ShaderCacheCoroutine(shaderCache, path);
        }

        public static void LoadShader(this ShaderCache shaderCache, TextReader reader, string key)
        {
            GLShader shader;
            shader = new GLShader(GetShaderTypeFromExtension(key), shaderCache.Context, key);
            try
            {
                shader.Compile(reader.ReadToEnd());
            }
            catch (Exception)
            {
                shader.Dispose();
                throw;
            }

            shaderCache.AddItem(key, shader);
        }
    }
}
