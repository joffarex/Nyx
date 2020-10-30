using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Nyx.Core.Scene;
using Silk.NET.OpenGL;

namespace Nyx.Core.Renderer
{
    public class DebugDraw
    {
        private static readonly int _maxLines = 500;

        private static readonly List<Line2D> _line2Ds = new List<Line2D>();

        // 6 float per vertex, 2 per line
        private static readonly float[] _vertexArray = new float[_maxLines * 6 * 2];
        private static readonly Shader _shader = AssetManager.GetShader("assets/shaders/debug-line-2d.glsl");

        private static BufferObject<float> _vertexBufferObject;
        private static VertexArrayObject<float> _vertexArrayobject;

        private static bool _startedOnGpu;

        public static void Start()
        {
            _vertexBufferObject =
                new BufferObject<float>(_vertexArray, BufferTargetARB.ArrayBuffer, BufferUsageARB.DynamicDraw);
            _vertexArrayobject = new VertexArrayObject<float>(_vertexBufferObject);

            // Position
            _vertexArrayobject.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 6, 0);
            // Color
            _vertexArrayobject.VertexAttributePointer(1, 3, VertexAttribPointerType.Float, 6, 3);
        }

        public static void Dispose()
        {
            _vertexBufferObject.Dispose();
            _vertexArrayobject.Dispose();
            _shader.Dispose();
        }

        public static void BeginFrame()
        {
            if (!_startedOnGpu)
            {
                Start();
                _startedOnGpu = true;
            }

            // Remove dead lines
            for (var i = 0; i < _line2Ds.Count; i++)
            {
                Line2D line = _line2Ds[i];
                if (line.BeginFrame() < 0)
                {
                    _line2Ds.Remove(line);
                    i--;
                }
            }
        }

        public static void Draw()
        {
            if (!_line2Ds.Any())
            {
                return;
            }

            var index = 0;
            foreach (Line2D line in _line2Ds)
            {
                // 2 is verticies
                for (var i = 0; i < 2; i++)
                {
                    Vector2 point = i == 0 ? line.From : line.To;
                    Vector3 color = line.Color;

                    _vertexArray[index] = point.X;
                    _vertexArray[index + 1] = point.Y;
                    _vertexArray[index + 2] = 0.0f;

                    _vertexArray[index + 3] = color.X;
                    _vertexArray[index + 4] = color.Y;
                    _vertexArray[index + 5] = color.Z;

                    index += 6;
                }

                int subDataSize = _line2Ds.Count * 6 * 2;
                var subData = new float[subDataSize];
                Array.Copy(_vertexArray, subData, subDataSize);
                _vertexBufferObject.Bind();
                _vertexBufferObject.ReBufferData(subData);

                _shader.Use();
                _shader.SetUniform("uProjection", SceneContext.CurrentScene.Camera2D.ProjectionMatrix);
                _shader.SetUniform("uView", SceneContext.CurrentScene.Camera2D.GetViewMatrix());

                _vertexArrayobject.Bind();
                _vertexArrayobject.EnableVertexAttribPointers();

                GraphicsContext.DrawArrays(PrimitiveType.Lines, (uint) subDataSize);

                _vertexArrayobject.DisableVertexAttribPointers();
                _vertexArrayobject.Detach();

                _shader.Detach();
            }
        }

        public static void AddLine2D(Vector2 from, Vector2 to)
        {
            AddLine2D(from, to, new Vector3(0, 1, 0), 1);
        }

        public static void AddLine2D(Vector2 from, Vector2 to, Vector3 color)
        {
            AddLine2D(from, to, color, 1);
        }

        public static void AddLine2D(Vector2 from, Vector2 to, Vector3 color, int lifeTime)
        {
            if (_line2Ds.Count >= _maxLines)
            {
                return;
            }

            _line2Ds.Add(new Line2D(from, to, color, lifeTime));
        }
    }
}