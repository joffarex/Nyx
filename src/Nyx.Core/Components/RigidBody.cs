using System.Numerics;
using Newtonsoft.Json;

namespace Nyx.Core.Components
{
    public class RigidBody : Component
    {
        [JsonRequired] private int _colliderType;
        [JsonRequired] private float _friction = 0.8f;

        public Vector3 Velocity { get; set; } = new Vector3(0, 0.5f, 0);

        public override void Update(float deltaTime)
        {
        }

        public override void Render()
        {
        }
    }
}