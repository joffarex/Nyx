using System.Drawing;
using System.Numerics;
using Nyx.Core;
using Nyx.Core.Components;
using Nyx.Core.OpenGL;
using Nyx.SharpTT;
using Silk.NET.Input.Common;

namespace Nyx.Playground
{
    public class LevelEditorScene : Scene
    {
        public override void Init()
        {
            // Camera3D = new Camera3D(Vector3.UnitZ * 6, Vector3.UnitZ * -1, Vector3.UnitY, (float) Width / Height);
            Camera2D = new Camera2D(new Vector2(-250.0f, 0.0f));

            var xOffset = 10;
            var yOffset = 10;

            float totalWidth = (float) 600 - (xOffset * 2);
            float totalHeight = (float) 300 - (yOffset * 2);
            float sizeX = totalWidth / 100.0f;
            float sizeY = totalHeight / 100.0f;
            const float padding = 0;

            for (var x = 0; x < 100; x++)
            {
                for (var y = 0; y < 100; y++)
                {
                    float xPos = xOffset + (x * sizeX) + (padding * x);
                    float yPos = yOffset + (y * sizeY) + (padding * y);

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
            // Camera3D.MoveCamera(deltaTime);

            base.Update(deltaTime);
        }

        public override void MouseMove(IMouse mouse, PointF position)
        {
            // Camera3D.MouseMove(LastMousePosition, position);
        }

        public override void MouseScroll(IMouse mouse, ScrollWheel scrollWheel)
        {
            // Camera3D.ModifyZoom(scrollWheel.Y);
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