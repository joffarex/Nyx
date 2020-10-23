using Nyx.Core.OpenGL;
using Nyx.Engine;
using Silk.NET.OpenGL;

namespace Nyx.Playground
{
    public class LevelEditorScene : Scene
    {
        private static BufferObject<float> _vertexBufferObject;
        private static BufferObject<uint> _elementBufferObject;
        private static VertexArrayObject<float, uint> _vertexArrayobject;

        private readonly uint[] _elementArray =
        {
            2, 1, 0, // Top right triangle,
            0, 1, 3, // Bottom left triangle
        };

        private Shader _shader;

        private readonly float[] _vertexArray =
        {
            // position        // color
            0.5f, -0.5f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f, // Bottom right 0
            -0.5f, 0.5f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f, // Top left 1
            0.5f, 0.5f, 0.0f, 0.0f, 0.0f, 1.0f, 1.0f, // Top right 2
            -0.5f, -0.5f, 0.0f, 1.0f, 1.0f, 0.0f, 1.0f, // Bottom left 3
        };

        public override void Init()
        {
            string shaderPath = NyxEngine.GetFullPath("Shaders/square.glsl");
            _shader = new Shader(NyxEngine.Gl, shaderPath);

            _vertexBufferObject = new BufferObject<float>(NyxEngine.Gl, _vertexArray, BufferTargetARB.ArrayBuffer);
            _elementBufferObject =
                new BufferObject<uint>(NyxEngine.Gl, _elementArray, BufferTargetARB.ElementArrayBuffer);
            _vertexArrayobject =
                new VertexArrayObject<float, uint>(NyxEngine.Gl, _vertexBufferObject, _elementBufferObject);

            _vertexArrayobject.SetVertexAttribPointers3Pos4Col();
            _vertexArrayobject.EnableVertexAttribPointers(new uint[] {0, 1});
        }

        public override void Update(float deltaTime)
        {
        }

        public override unsafe void Render()
        {
            _shader.Use();
            _vertexArrayobject.Bind();

            _vertexArrayobject.EnableVertexAttribPointers(new uint[] {0, 1});

            NyxEngine.Gl.DrawElements(PrimitiveType.Triangles, (uint) _elementArray.Length,
                DrawElementsType.UnsignedInt, null);

            _vertexArrayobject.DisableVertexAttribPointers(new uint[] {0, 1});

            NyxEngine.Gl.BindVertexArray(0);
            NyxEngine.Gl.UseProgram(0);
        }

        public override void Dispose()
        {
            _vertexBufferObject.Dispose();
            _elementBufferObject.Dispose();
            _vertexArrayobject.Dispose();
            _shader.Dispose();
        }
    }
}