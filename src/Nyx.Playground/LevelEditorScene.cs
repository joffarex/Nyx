using System.Numerics;
using Nyx.Core.Components;
using Nyx.Core.OpenGL;
using Nyx.Core.Renderer;
using Nyx.Core.Scene;
using Nyx.Core.Utils;
using Nyx.SharpTT;

namespace Nyx.Playground
{
    public class LevelEditorScene : Scene
    {
        public override void Init()
        {
            Camera2D = new Camera2D(new Vector2(-250.0f, 0.0f));

            var gameObject = new GameObject("Object 1",
                new Transform(new Vector2(100.0f, 100.0f), new Vector2(256.0f, 256.0f)));
            gameObject.AddComponent(new SpriteRenderer(AssetManager.GetTexture("assets/sprites/mario.png")));
            AddGameObjectToScene(gameObject);

            LoadResources();
        }

        private void GenerateGradientQuad()
        {
            const int xOffset = 10;
            const int yOffset = 10;

            const float totalWidth = (float) 600 - (xOffset * 2);
            const float totalHeight = (float) 300 - (yOffset * 2);
            const float sizeX = totalWidth / 100.0f;
            const float sizeY = totalHeight / 100.0f;
            const float padding = 0;

            for (var x = 0; x < 100; x++)
            {
                for (var y = 0; y < 100; y++)
                {
                    float xPos = xOffset + (x * sizeX) + (padding * x);
                    float yPos = yOffset + (y * sizeY) + (padding * y);

                    var gameObject = new GameObject($"GameObject(${x}:${y})",
                        new Transform(new Vector2(xPos, yPos), new Vector2(sizeX, sizeY)));
                    gameObject.AddComponent(
                        new SpriteRenderer(new Vector4(xPos / totalWidth, yPos / totalHeight, 0.5f, 0.1f)));
                    AddGameObjectToScene(gameObject);
                }
            }
        }

        private void LoadResources()
        {
            AssetManager.GetShader("assets/shaders/default.glsl");
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
    }
}