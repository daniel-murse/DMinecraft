using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Lines;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Pipeline.Mixed2D;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Shaders.Content;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites.Batches;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites.Renderers;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Text;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Text.Content;
using DMinecraft.PhysicalClient.Scheduling.Coroutines;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Scenes.Init
{
    internal class InitScene : IScene
    {
        FontBuilderFactory fontBuilderFactory;
        FontBuilder debugFontBuilder;
        FontCreationCoroutine debugFontCo;

        ShaderCache shaderCache;

        GLProgram spriteSdfProgram;

        public InitScene(GLContext glContext, InitSceneSettings settings)
        {
            fontBuilderFactory = new FontBuilderFactory(settings.ContentRoot);
            debugFontBuilder = fontBuilderFactory.CreateBuilder("font/ttf/Monospace.ttf");
            debugFontCo = debugFontBuilder.BuildCreationCoroutine(16, 33, 126, true, glContext);
            debugFontCo.Complete();
            var debugFont = debugFontCo.Font;
            
        
            shaderCache = new ShaderCache(glContext);
            shaderCache.CreateCoroutine(settings.ContentRoot + "shader/").Complete();

            GLProgram spriteSdfProgram = new GLProgram(glContext, "sprite_sdf");
            spriteSdfProgram.Link(shaderCache.GetVertexShader("sprite/sdf/"), shaderCache.GetFragmentShader("sprite/sdf/"));
            GLProgram spriteProgram = new GLProgram(glContext, "sprite");
            spriteProgram.Link(shaderCache.GetVertexShader("sprite/"), shaderCache.GetFragmentShader("sprite/"));
            GLProgram lineProgram = new GLProgram(glContext, "line");
            lineProgram.Link(shaderCache.GetVertexShader("line/"), shaderCache.GetFragmentShader("line/"));

            SpriteRenderer spriteRenderer = new SpriteRenderer(spriteProgram);
            SdfSpriteRenderer sdfSpriteRenderer = new SdfSpriteRenderer(spriteSdfProgram);
            LineRenderer lineRenderer = new LineRenderer(lineProgram);

            Mixed2DDrawStage mixed2D = new Mixed2DDrawStage(null, null, null);

            var dfr = new FontSpriteResources() { Font = debugFont, RenderBatch = null};
            //mixed2D.SpriteRenderBatches.Add()
        }

        public void Update(TimeSpan deltaTime)
        {
        }

        public void Render(TimeSpan deltaTime)
        {
        }

        public void OnClientSizeChanged(Vector2i newSize)
        {
        }
    }
}
