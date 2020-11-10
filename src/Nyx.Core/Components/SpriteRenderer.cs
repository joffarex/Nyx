using System.Numerics;
using Newtonsoft.Json;
using Nyx.Core.Renderer;

namespace Nyx.Core.Components
{
    public class SpriteRenderer : Component
    {
        private Vector4 _color;


        private Sprite _sprite;

        public SpriteRenderer()
        {
        }

        public SpriteRenderer(Vector4 color, int width, int height)
        {
            Color = color;
            Sprite = new(width, height);
            IsDirty = true;
            Texture = Sprite.Texture;
        }

        public SpriteRenderer(Sprite sprite)
        {
            Sprite = sprite;
            Color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            IsDirty = true;
            Texture = Sprite.Texture;
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

        [JsonIgnore] public Vector2[] TextureCoordinates => Sprite.TextureCoordinates;

        public Sprite Sprite
        {
            get => _sprite;
            set
            {
                _sprite = value;
                IsDirty = true;
            }
        }

        [JsonIgnore] public Transform LastTransform { get; set; }

        [JsonIgnore] public bool IsDirty { get; set; } = true;

        public Texture Texture { get; set; }

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

        public override void ImGui()
        {
            if (ImGuiNET.ImGui.ColorPicker4("Color Picker: ", ref _color))
            {
                IsDirty = true;
            }
        }
    }
}