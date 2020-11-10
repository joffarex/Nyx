using System;
using System.Numerics;
using Nyx.Core.Components;
using Nyx.Core.Renderer;
using Nyx.Core.Utils;

namespace Nyx.Core.Scene
{
    public class LevelEditorScene : Scene
    {
        private const string SpriteSheetPath = "assets/sprites/spritesheets/decorationsAndBlocks.png";

        private readonly GameObject _levelEditorGameObjects =
            new GameObject("LevelEditor", new Transform(new Vector2()), 0);

        private SpriteSheet _spriteSheet;

        public override void Init()
        {
            _levelEditorGameObjects.AddComponent(new MouseControl());
            _levelEditorGameObjects.AddComponent(new GridLines());

            Camera2D = new(new Vector2(-250.0f, 0.0f), SceneContext.BaseSize);
            _spriteSheet = AssetManager.GetSpriteSheet(SpriteSheetPath);
            if (LevelLoaded)
            {
                ActiveGameObject = GameObjects.Find(g => g.Name is "Object 1");
            }

            // _gameObject1 = new GameObject("Object 1",
            // new Transform(new Vector2(200.0f, 100.0f), new Vector2(256.0f, 256.0f)), 2);
            // _gameObject1.AddComponent(
            // new SpriteRenderer(new Vector4(1, 0, 0, 1), 256, 256));
            // _gameObject1.AddComponent(new RigidBody());
            // _gameObject1.AddComponent(new RigidBody());
            // AddGameObjectToScene(_gameObject1);
            // var gameObject2 = new GameObject("Object 2",
            // new Transform(new Vector2(400.0f, 100.0f), new Vector2(256.0f, 256.0f)), 4);
            // gameObject2.AddComponent(
            // new SpriteRenderer(new Sprite(AssetManager.GetTexture("assets/sprites/blendImage2.png"))));
            // AddGameObjectToScene(gameObject2);

            // ActiveGameObject = _gameObject1;
        }

        public override void LoadResources()
        {
            AssetManager.GetShader("assets/shaders/default.glsl");
            AssetManager.GetTexture("assets/sprites/blendImage2.png");

            Texture spriteSheetTexture = AssetManager.GetTexture(SpriteSheetPath);
            AssetManager.AddSpriteSheet(SpriteSheetPath, new(spriteSheetTexture, 16, 16, 81, 0));
        }

        public override void Update(float deltaTime)
        {
            Fps.Print(deltaTime);

            _levelEditorGameObjects.Update(deltaTime);

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
            ImGuiNET.ImGui.Begin("Window");

            Vector2 windowPos = ImGuiNET.ImGui.GetWindowPos();
            Vector2 windowSize = ImGuiNET.ImGui.GetWindowSize();
            Vector2 itemSpacing = ImGuiNET.ImGui.GetStyle().ItemSpacing;

            float windowX2 = windowPos.X + windowSize.X; // Right most coordinate on screen

            for (var i = 0; i < _spriteSheet.Count(); i++)
            {
                Sprite sprite = _spriteSheet.Sprites[i];
                float spriteWidth = sprite.Width * 2;
                float spriteHeight = sprite.Height * 2;
                var spriteSizeForGui = new Vector2(spriteWidth * 2, spriteHeight * 2);
                uint id = sprite.GetTextureHandle();
                Vector2[] textureCoordinates = sprite.TextureCoordinates;

                var uv0 = new Vector2(textureCoordinates[2].X, textureCoordinates[0].Y);
                var uv1 = new Vector2(textureCoordinates[0].X, textureCoordinates[2].Y);

                ImGuiNET.ImGui.PushID(i);
                if (ImGuiNET.ImGui.ImageButton((IntPtr) id, spriteSizeForGui, uv0, uv1))
                {
                    GameObject gameObject = Prefab.GenerateSpriteObject(sprite, spriteWidth, spriteHeight);
                    _levelEditorGameObjects.GetComponent<MouseControl>().PickUpObject(gameObject);
                }

                ImGuiNET.ImGui.PopID();

                Vector2 lastButtonPos = ImGuiNET.ImGui.GetItemRectMax();
                float lastButtonX2 = lastButtonPos.X;
                float nextButtonX2 = lastButtonX2 + itemSpacing.X + spriteSizeForGui.X;
                if (((i + 1) < _spriteSheet.Count()) && (nextButtonX2 < windowX2))
                {
                    ImGuiNET.ImGui.SameLine();
                }
            }


            ImGuiNET.ImGui.End();
        }
    }
}