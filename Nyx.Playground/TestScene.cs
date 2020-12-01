using Nyx.Core.Components;
using Nyx.Core.Entities;
using Nyx.Core.Renderer;
using Nyx.Core.Scenes;
using Nyx.Core.Shared;
using OpenTK.Mathematics;
using Vector4 = System.Numerics.Vector4;

namespace Nyx.Playground
{
    public class TestScene : Scene
    {
        private Entity _testEntity;

        public override void Init()
        {
            Camera2D = new Camera2D(new Vector2(-250.0f, 0.0f), SceneManager.BaseSize);
            _testEntity = new Entity("Test",
                new Transform(new System.Numerics.Vector2(200.0f, 100.0f), new System.Numerics.Vector2(256.0f, 256.0f)),
                2);
            _testEntity.AddComponent(new SpriteRenderer(new Vector4(1, 0, 0, 1), 256, 256));
            AddEntityToScene(_testEntity);

            ActiveEntity = _testEntity;
        }

        public override void LoadResources()
        {
        }
    }
}