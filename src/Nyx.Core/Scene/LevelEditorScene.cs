using System;
using System.Numerics;
using Nyx.Core.Components;
using Nyx.Core.Event;
using Nyx.Core.Renderer;
using Nyx.Ecs;

namespace Nyx.Core.Scene
{
    public class LevelEditorScene : Scene
    {
        private const string SpriteSheetPath = "assets/sprites/spritesheets/decorationsAndBlocks.png";

        private GameObject _gameObject1;

        private SpriteSheet _spriteSheet;

        public override void Init()
        {
            Camera2D = new Camera2D(new Vector2(-250.0f, 0.0f), SceneContext.BaseSize);
            _spriteSheet = AssetManager.GetSpriteSheet(SpriteSheetPath);
            if (LevelLoaded)
            {
                ActiveGameObject = GameObjects.Find(g => g.Name == "Object 1");
                return;
            }

            _gameObject1 = new GameObject("Object 1",
                new Transform(new Vector2(200.0f, 100.0f), new Vector2(256.0f, 256.0f)), 2);
            _gameObject1.AddComponent(
                new SpriteRenderer(new Vector4(1, 0, 0, 1), 256, 256));
            _gameObject1.AddComponent(new RigidBody());
            AddGameObjectToScene(_gameObject1);
            var gameObject2 = new GameObject("Object 2",
                new Transform(new Vector2(400.0f, 100.0f), new Vector2(256.0f, 256.0f)), 4);
            gameObject2.AddComponent(
                new SpriteRenderer(new Sprite(AssetManager.GetTexture("assets/sprites/blendImage2.png"))));
            AddGameObjectToScene(gameObject2);

            ActiveGameObject = _gameObject1;
        }

        public override void LoadResources()
        {
            AssetManager.GetShader("assets/shaders/default.glsl");
            AssetManager.GetTexture("assets/sprites/blendImage2.png");

            Texture spriteSheetTexture = AssetManager.GetTexture(SpriteSheetPath);
            AssetManager.AddSpriteSheet(SpriteSheetPath, new SpriteSheet(spriteSheetTexture, 16, 16, 81, 0));
        }

        public override void Update(float deltaTime)
        {
            // Fps.Print(deltaTime);

            float orthoX = EventContext.MouseEvent.GetOrthoX();
            float orthoY = EventContext.MouseEvent.GetOrthoY();

            Console.WriteLine($"X: {orthoX}, Y: {orthoY}");

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
                var spriteSize = new Vector2(spriteWidth, spriteHeight);
                uint id = sprite.GetTextureHandle();
                Vector2[] textureCoordinates = sprite.TextureCoordinates;

                ImGuiNET.ImGui.PushID(i);
                if (ImGuiNET.ImGui.ImageButton((IntPtr) id, spriteSize, textureCoordinates[0], textureCoordinates[2]))
                {
                    Console.WriteLine($"Button {i} clicked");
                }

                ImGuiNET.ImGui.PopID();

                Vector2 lastButtonPos = ImGuiNET.ImGui.GetItemRectMax();
                float lastButtonX2 = lastButtonPos.X;
                float nextButtonX2 = lastButtonX2 + itemSpacing.X + spriteSize.X;
                if (((i + 1) < _spriteSheet.Count()) && (nextButtonX2 < windowX2))
                {
                    ImGuiNET.ImGui.SameLine();
                }
            }


            ImGuiNET.ImGui.End();
        }
    }
}