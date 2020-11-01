using System.Numerics;
using Nyx.Core;
using Nyx.Core.Components;
using Nyx.Core.Renderer;
using Nyx.Core.Scene;

namespace Nyx.Playground
{
    public class AnimationScene : Scene
    {
        private const string SpriteSheetPath = "assets/sprites/spritesheets/adventure.png";

        private AnimatedSprite _animatedSprite;

        private SpriteSheet _spriteSheet;

        public override void Init()
        {
            Camera2D = new Camera2D(new Vector2(-250.0f, 0.0f), SceneContext.BaseSize);
            _spriteSheet = AssetManager.GetSpriteSheet(SpriteSheetPath);

            // TODO: disabled temporarily, required fixing of gameobject serialization
            // if (LevelLoaded)
            // {
            // _animatedSprite = GetGameObject<AnimatedSprite>("AnimatedSprite");
            // ActiveGameObject = _animatedSprite;
            // return;
            // }

            _animatedSprite = new AnimatedSprite(_spriteSheet, 0.1f,
                new Vector2(200.0f, 100.0f), 10, 2);
            _animatedSprite.AddAnimation("idle1", new[] {0, 1, 2, 3});

            AddGameObjectToScene(_animatedSprite);

            ActiveGameObject = _animatedSprite;
        }

        public override void LoadResources()
        {
            AssetManager.GetShader("assets/shaders/default.glsl");
            Texture texture = AssetManager.GetTexture(SpriteSheetPath);
            AssetManager.AddSpriteSheet(SpriteSheetPath,
                new SpriteSheet(texture, 50, 37, 109, 0));
        }

        public override void Update(float deltaTime)
        {
            // Fps.Print(deltaTime);

            _animatedSprite.PlayAnimation("idle1", deltaTime);

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
    }
}