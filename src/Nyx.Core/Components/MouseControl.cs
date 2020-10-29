using System.Numerics;
using Nyx.Core.Event;
using Nyx.Core.Scene;
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
                HoldingGameObject.Transform.Position = new Vector2(EventContext.MouseEvent.GetOrthoX() - 16,
                    EventContext.MouseEvent.GetOrthoY() - 16);

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