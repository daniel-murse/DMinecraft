using DMinecraft.PhysicalClient.Content;
using DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Lines;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Pipeline;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Shaders.Content;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Sprites.Packed;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Text.Content;
using DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Textures.Atlas;
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
                new GlyphRangeCreationCoroutine(ftFace, 16, new GlyphRange(1, 63), false, atlas)
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

            

            
        }

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
            Matrix4 matrix = Matrix4.CreateScale((float)Math.Sin(stopwatch.Elapsed.TotalSeconds));
            matrix = Matrix4.Identity;
            spriteRenderer = new SpriteRenderer(spriteProgram);
            spriteRenderer.Transform.Set(ref matrix);
            spriteRenderer.Albedo.Set(0);
            program.Context.BindTexture2DArray(atlas.Texture, 0);
            atlas.Texture.MinFilter = TextureMinFilter.Linear;
            atlas.Texture.MagFilter = TextureMagFilter.Linear;
            renderPipeline.Render(deltaTime);

            spriteBatch.Clear();

            Graphics.OpenGL.HighLevel.Sprites.Sprite sprite = new Graphics.OpenGL.HighLevel.Sprites.Sprite();
            sprite.Position = new Vector3(-1,-1,0);
            sprite.Scale = Vector3.One;
            sprite.Size = new Vector3(2, 2, 0);
            sprite.Color = Color4.Red;
            sprite.Texture = new PackedTexture2DArrayAtlasItem(atlas, 0, 0, 255, 255, 0);


            var vertices = spriteBatch.SubmitSprites(1);
            sprite.ComputeVertices(ref vertices[0]);
            

            spriteBatch.Draw();
        }

        public void Update(TimeSpan deltaTime)
        {
            //todo pass down from app or smth
            //coroutine.Step(TimeSpan.FromMilliseconds(8));
        }
    }
}
