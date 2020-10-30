using System.Numerics;
using Nyx.Core.Renderer;
using Nyx.Core.Scene;
using Nyx.Core.Utils;
using Nyx.Ecs;

namespace Nyx.Core.Components
{
    public class GridLines : Component
    {
        public override void Update(float deltaTime)
        {
            Vector2 cameraPosition = SceneContext.CurrentScene.Camera2D.Position;
            Vector2 projectionSize = SceneContext.BaseSize;

            int firstX = ((int) (cameraPosition.X / Settings.GridWidth) - 1) * Settings.GridWidth;
            int firstY = ((int) (cameraPosition.Y / Settings.GridHeight) - 1) * Settings.GridHeight;

            int numVerticalLines = (int) (projectionSize.X / Settings.GridWidth) + 2;
            int numHorizontalLines = (int) (projectionSize.Y / Settings.GridHeight) + 2;

            int width = (int) projectionSize.X + Settings.GridWidth * 2;
            int height = (int) projectionSize.Y + Settings.GridHeight * 2;

            int maxLines = System.Math.Max(numVerticalLines, numHorizontalLines);
            var color = new Vector3(0.2f, 0.2f, 0.2f);

            for (var i = 0; i < maxLines; i++)
            {
                int x = firstX + (Settings.GridWidth * i);
                int y = firstY + (Settings.GridHeight * i);

                if (i < numVerticalLines)
                {
                    DebugDraw.AddLine2D(new Vector2(x, firstY), new Vector2(x, y + height), color);
                }

                if (i < numHorizontalLines)
                {
                    DebugDraw.AddLine2D(new Vector2(firstX, y), new Vector2(x + width, y), color);
                }
            }
        }

        public override void Render()
        {
        }
    }
}