using System.Drawing;
using System.Numerics;
using Nyx.Core;
using Nyx.Core.Components;
using Nyx.Core.OpenGL;
using Nyx.SharpTT;
using Silk.NET.Input.Common;
using static Nyx.Core.Game;
using static Nyx.Core.NyxEngine;

namespace Nyx.Playground
{
    public class LevelEditorScene : Scene
    {
        public override void Init()
        {
            // TODO: fix camera to for 2D
            
            Camera = new Camera(Vector3.UnitZ * 6, Vector3.UnitZ * -1, Vector3.UnitY, (float) Width / Height);
            // Camera = new Camera(Vector3.Zero, new Vector3(0.0f, 0.0f, -1.0f), new Vector3(0.0f, 1.0f, 0.0f),
            // (float) Width / Height);

            var xOffset = 10;
            var yOffset = 10;

            float totalWidth = Width - (xOffset * 2);
            float totalHeight = Height - (yOffset * 2);
            float sizeX = totalWidth / 100.0f;
            float sizeY = totalHeight / 100.0f;

            for (var x = 0; x < 100; x++)
            {
                for (var y = 0; y < 100; y++)
                {
                    float xPos = xOffset + (x * sizeX);
                    float yPos = yOffset + (y * sizeY);

                    var gameObject = new GameObject($"Obj ${x} ${y}",
                        new Transform(new Vector2(xPos, yPos), new Vector2(sizeX, sizeY)));
                    gameObject.AddComponent(
                        new SpriteRenderer(new Vector4(xPos / totalWidth, yPos / totalHeight, 1, 1)));
                    AddGameObjectToScene(gameObject);
                }
            }
        }

        public override void Update(float deltaTime)
        {
            float moveSpeed = 2.5f * deltaTime;

            if (Input.IsKeyPressed(Key.W))
            {
                Camera.Position += moveSpeed * Camera.Front;
            }

            if (Input.IsKeyPressed(Key.S))
            {
                Camera.Position -= moveSpeed * Camera.Front;
            }

            if (Input.IsKeyPressed(Key.A))
            {
                Camera.Position -= Vector3.Normalize(Vector3.Cross(Camera.Front, Camera.Up)) * moveSpeed;
            }

            if (Input.IsKeyPressed(Key.D))
            {
                Camera.Position += Vector3.Normalize(Vector3.Cross(Camera.Front, Camera.Up)) * moveSpeed;
            }

            base.Update(deltaTime);
        }

        public override void MouseMove(IMouse mouse, PointF position)
        {
            const float lookSensitivity = 0.1f;
            if (LastMousePosition == default)
            {
                LastMousePosition = position;
            }
            else
            {
                float xOffset = (position.X - LastMousePosition.X) * lookSensitivity;
                float yOffset = (position.Y - LastMousePosition.Y) * lookSensitivity;
                LastMousePosition = position;

                Camera.ModifyDirection(xOffset, yOffset);
            }
        }

        public override void MouseScroll(IMouse mouse, ScrollWheel scrollWheel)
        {
            Camera.ModifyZoom(scrollWheel.Y);
        }

        public override void Render()
        {
            Renderer.Render();

            base.Render();
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}