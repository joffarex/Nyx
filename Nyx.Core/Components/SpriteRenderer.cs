#nullable enable
using System.Numerics;
using System.Text.Json.Serialization;
using Nyx.Core.Renderer;
using Nyx.Core.Shared;

namespace Nyx.Core.Components
{
    public class SpriteRenderer : Component
    {
        private Vector4 _color;
        private Sprite _sprite = null!;

        public SpriteRenderer()
        {
        }

        public SpriteRenderer(Vector4 color, int width, int height)
        {
            Init(color, new Sprite(width, height));
        }

        public SpriteRenderer(Sprite sprite)
        {
            Init(new Vector4(1.0f, 1.0f, 1.0f, 1.0f), sprite);
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

        [JsonIgnore] public Transform LastTransform { get; private set; } = null!;

        [JsonIgnore] public bool IsDirty { get; set; }

        public Texture Texture { get; private set; } = null!;

        public void Init(Vector4 color, Sprite sprite)
        {
            Sprite = sprite;
            Color = color;
            Texture = Sprite.Texture;
            IsDirty = true;
        }


        public override void Dispose()
        {
        }

        public override void Start()
        {
            LastTransform = Entity.Transform.Copy();
        }

        public override void Update(ref double deltaTime)
        {
            if (!LastTransform.Equals(Entity.Transform))
            {
                Entity.Transform.Copy(LastTransform);
                IsDirty = true;
            }
        }

        public override void Render(ref double deltaTime)
        {
        }

        public override void ImGui()
        {
            ImGuiNET.ImGui.Begin("Picker");
            if (ImGuiNET.ImGui.ColorPicker4("Color Picker: ", ref _color))
            {
                IsDirty = true;
            }

            ImGuiNET.ImGui.End();

            base.ImGui();
        }
    }
}