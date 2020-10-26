using System.Numerics;
using Nyx.Core.OpenGL;
using Nyx.Core.Renderer;
using Nyx.SharpTT;

namespace Nyx.Core.Components
{
    public class SpriteRenderer : Component
    {
        private Vector4 _color;


        private Sprite _sprite;

        public SpriteRenderer(Vector4 color)
        {
            Color = color;
            Sprite = new Sprite(null);
        }

        public SpriteRenderer(Sprite sprite)
        {
            Sprite = sprite;
            Color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
        }

        public Vector4 Color
        {
            get => _color;
            set
            {
                if (_color != value)
                {
                    _color = value;
                    IsDirty = true;
                }
            }
        }

        public Vector2[] TextureCoordinates => Sprite.TextureCoordinates;

        public Sprite Sprite
        {
            get => _sprite;
            set
            {
                _sprite = value;
                IsDirty = true;
            }
        }

        public Transform LastTransform { get; set; }

        public bool IsDirty { get; set; }

        public Texture Texture => Sprite.Texture;

        public override void Start()
        {
            LastTransform = GameObject.Transform.Copy();
        }


        public override void Update(float deltaTime)
        {
            if (!LastTransform.Equals(GameObject.Transform))
            {
                GameObject.Transform.Copy(LastTransform);
                IsDirty = true;
            }
        }

        public override void Render()
        {
        }
    }
}