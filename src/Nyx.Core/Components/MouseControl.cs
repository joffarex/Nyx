using System.Numerics;
using Nyx.Core.Event;
using Nyx.Core.Scene;
using Nyx.Core.Utils;
using Nyx.Ecs;
using Silk.NET.Input.Common;

namespace Nyx.Core.Components
{
    public class MouseControl : Component
    {
        public GameObject HoldingGameObject { get; set; }


        public void PickUpObject(GameObject gameObject)
        {
            HoldingGameObject = gameObject;
            SceneContext.CurrentScene.AddGameObjectToScene(gameObject);
        }

        public void PlaceObject()
        {
            HoldingGameObject = null;
        }

        public override void Update(float deltaTime)
        {
            if (HoldingGameObject != null)
            {
                float xPos = EventContext.MouseEvent.GetOrthoX() - 16;
                float yPos = EventContext.MouseEvent.GetOrthoY() - 16;

                xPos = (float) System.Math.Round(xPos / Settings.GridWidth) * Settings.GridWidth;
                yPos = (float) System.Math.Round(yPos / Settings.GridHeight) * Settings.GridHeight;

                HoldingGameObject.Transform.Position = new Vector2(xPos, yPos);

                if (EventContext.MouseEvent.IsButtonPressed(MouseButton.Left))
                {
                    PlaceObject();
                }
            }
        }

        public override void Render()
        {
        }
    }
}