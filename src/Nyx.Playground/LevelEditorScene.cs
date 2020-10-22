using Nyx.Core.OpenGL;
using Nyx.Engine;

namespace Nyx.Playground
{
    public class LevelEditorScene : Scene
    {
        private Shader _shader;

        public override void Init()
        {
            string shaderPath = NyxEngine.GetFullPath("Shaders/square.glsl");
            _shader = new Shader(NyxEngine.Gl, shaderPath);
        }

        public override void Update(float deltaTime)
        {
        }

        public override void Render()
        {
            _shader.Use();
        }

        public override void Dispose()
        {
            _shader.Dispose();
        }
    }
}