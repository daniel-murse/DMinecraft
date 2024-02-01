using DMinecraft.PhysicalClient.Content;
using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Lines;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Pipeline;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Pipeline.Mixed2D;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Shaders.Content;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites.Debug;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Text;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Text.Content;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Textures.Atlas;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Util;
using DMinecraft.PhysicalClient.Scheduling.Coroutines;
using FreeTypeBinding;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Scenes.Init
{
    internal class InitScene : IScene
    {
        public unsafe InitScene(GLContext glContext, InitSceneSettings settings)
        {
            glContext.SetUnpackAlignment(1);
            atlas = new PackedTexture2DArrayAtlas(glContext, new PackedTexture2DArrayAtlasOptions()
            {
                ImageWidth = 32,
                ImageHeight = 32,
                InternalFormat = (InternalFormat)SizedInternalFormat.R8,
                MipLevels = 1,
                Cols = 16,
                Rows = 16,
                Depth = 16,
            });

            FT_LibraryRec_* ft;
            FT.FT_Init_FreeType(&ft);
            FT_FaceRec_* ftFace;
            var bytes = Encoding.ASCII.GetBytes((settings.ContentRoot + "/font/otf/NanumSquareNeoOTF-Rg.otf"));
            fixed (byte* ptr = bytes)
            {
                FT.FT_New_Face(ft, ptr, 0, &ftFace);
            }

            ShaderCache = new ShaderCache(glContext);
            Settings = settings;
            var coroutines = new ITimedCoroutine[]
            {
                new ShaderCacheCoroutine(ShaderCache, Path.Combine(settings.ContentRoot, "shader/")),
                new FTGlyphRangeCreationCoroutine(ftFace, 1, 63, false, atlas)
            };
            coroutine = new SequentialCoroutinesCoroutine(coroutines.AsEnumerable().GetEnumerator());
            coroutine.Complete();

            program = new GLProgram(glContext, "debug_lb");
            program.Link(ShaderCache.GetVertexShader("line/"), ShaderCache.GetFragmentShader("line/"));
            lineRenderer = new LineRenderer(program);
            lineBatch = new LineBatch(glContext, 32, lineRenderer.VertexDeclaration, "debug_lb");
            var stage = new LineDrawStage(lineBatch);
            stage.Draw += LineVerticesUtil.TestDraw;
            renderPipeline = new RenderPipeline();
            renderPipeline.Stages.Add(new ClearFramebufferStage() { ClearBufferMask = ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit });
            //renderPipeline.Stages.Add(stage);
            spriteProgram = new GLProgram(glContext, "sprite_packed");
            spriteProgram.Link(ShaderCache.GetVertexShader("sprite/"), ShaderCache.GetFragmentShader("sprite/"));

            glContext.UseProgram(spriteProgram);
            spriteBatch = new SpriteBatch(glContext, 32, new SpriteVertexDeclaration(spriteProgram.Interface.Input), "sprite_packed");
            spriteBatch.Mesh.Vao.Use();

            //glContext.UseProgram(program);

            debugSpriteStage = new DebugSpriteDrawStage(spriteBatch, new SpriteRenderer(spriteProgram));

            //renderPipeline.Stages.Add(debugSpriteStage);


            debugSpriteStage.Draw += DebugSpriteStage_Draw;

            using (var factory = new FontBuilderFactory(settings.ContentRoot))
            {
                using (var builder = factory.CreateBuilder("font/otf/NanumSquareNeoOTF-Rg.otf"))
                {
                    using (var coroutine = builder.BuildCreationCoroutine(32, 1, 60, false, glContext))
                    {
                        coroutine.Complete();
                        if(!coroutine.IsCompleted)
                        {
                            throw new Exception();
                        }
                        debugFont = coroutine.Font;
                    };
                }
            };


            spriteRenderer = new SpriteRenderer(spriteProgram);

            var mixedSage = new Mixed2DDrawStage() { GLContext = glContext, SpriteBatch = spriteBatch, SpriteRenderer = spriteRenderer, LineBatch = lineBatch, LineRenderer = lineRenderer };
            renderPipeline.Stages.Add(mixedSage);
            mixedSage.OnDraw += MixedSage_OnDraw;

        }

        private void MixedSage_OnDraw(Mixed2DDrawStage obj)
        {
            var transform = new Transform();
            //transform.Scale *= 5;
            obj.GLContext.BindTexture2DArray(debugFont.Texture, 0);
            obj.SpriteRenderer.Albedo.Set(0);
            obj.SpriteRenderer.Program.Use();
            TextBuffer textBuffer = new TextBuffer();
            textBuffer.SetText(debugFont, "Hangul script\nIs cool");
            transform.CenterOrigin(textBuffer.BoundsSize);
            textBuffer.Submit(transform, obj.SpriteBatch, Color4.Gold);
        }

        private void DebugSpriteStage_Draw(DebugSpriteDrawStage obj)
        {
            obj.SpriteBatch.ChangeTexture2DArray(atlas.Texture, 0);
            obj.SpriteBatch.SubmitSpritesAF(1);
        }

        private DebugSpriteDrawStage debugSpriteStage;

        private Font debugFont;

        public ShaderCache ShaderCache { get; }
        public InitSceneSettings Settings { get; }

        private SequentialCoroutinesCoroutine coroutine;

        private RenderPipeline renderPipeline;
        private LineBatch lineBatch;

        private GLProgram program;
        GLProgram spriteProgram;
        LineRenderer lineRenderer;

        SpriteRenderer spriteRenderer;

        SpriteBatch spriteBatch;

        Stopwatch stopwatch = Stopwatch.StartNew();

        PackedTexture2DArrayAtlas atlas;

        public void Render(TimeSpan deltaTime)
        {
            GL.Enable(EnableCap.Blend);
            GL.Disable(EnableCap.DepthTest);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            
            renderPipeline.Render(deltaTime);
        }

        public void Update(TimeSpan deltaTime)
        {
            //todo pass down from app or smth
            //coroutine.Step(TimeSpan.FromMilliseconds(8));
        }

        public void OnClientSizeChanged(Vector2i newSize)
        {
            GL.Viewport(0, 0, newSize.X, newSize.Y);
            var halfSize= newSize / 2;
            Matrix4 matrix = Matrix4.CreateScale((float)Math.Sin(stopwatch.Elapsed.TotalSeconds));
            matrix = Matrix4.CreateOrthographicOffCenter(-halfSize.X, halfSize.X, -halfSize.Y, halfSize.Y, -1, 1);
            spriteRenderer.Transform.Set(ref matrix);
        }
    }
}
