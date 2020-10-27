using System.Numerics;
using Nyx.Core;
using Nyx.Core.Components;
using Nyx.Core.OpenGL;
using Nyx.Core.Renderer;
using Nyx.Core.Scene;
using Nyx.SharpTT;
using Nyx.Utils;

namespace Nyx.Playground
{
    public class LevelEditorScene : Scene
    {
        private const string SpriteSheetPath = "assets/sprites/spritesheet.png";

        private GameObject _gameObject1;
        private SpriteSheet _spriteSheet;

        public override void Init()
        {
            LoadResources();

            Camera2D = new Camera2D(new Vector2(-250.0f, 0.0f));

            _spriteSheet = AssetManager.GetSpriteSheet(SpriteSheetPath);

            _gameObject1 = new GameObject("Object 1",
                new Transform(new Vector2(200.0f, 100.0f), new Vector2(256.0f, 256.0f)), 2);
            _gameObject1.AddComponent(
                new SpriteRenderer(new Vector4(1, 0, 0, 1)));
            AddGameObjectToScene(_gameObject1);
            var gameObject2 = new GameObject("Object 2",
                new Transform(new Vector2(400.0f, 100.0f), new Vector2(256.0f, 256.0f)), 4);
            gameObject2.AddComponent(
                new SpriteRenderer(new Sprite(AssetManager.GetTexture("assets/sprites/blendImage2.png"))));
            AddGameObjectToScene(gameObject2);

            ActiveGameObject = _gameObject1;
        }

        private static void LoadResources()
        {
            AssetManager.GetShader("assets/shaders/default.glsl");

            Texture texture = AssetManager.GetTexture(SpriteSheetPath);
            AssetManager.AddSpriteSheet(SpriteSheetPath,
                new SpriteSheet(texture, 16, 16, 26, 0)
            );
        }

        public override void Update(float deltaTime)
        {
            Fps.Print(deltaTime);

            base.Update(deltaTime);
        }

        public override void Render()
        {
            BatchRenderer.Render();

            base.Render();
        }

        public override void Dispose()
        {
            BatchRenderer.Dispose();

            base.Dispose();
        }

        public override void ImGui()
        {
        }
    }
}