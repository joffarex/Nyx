using System;
using System.Collections.Generic;
using System.Linq;
using Nyx.Core.Graphics;
using Nyx.Core.Scenes;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Nyx.Core.Renderer
{
    public class DebugDraw
    {
        private const int MaxLines = 500;

        private static readonly List<Line2D> Line2Ds = new();

        // 6 float per vertex, 2 per line
        private static readonly float[] VertexArray = new float[MaxLines * 6 * 2];
        private static readonly Shader Shader = AssetManager.GetShader("assets/shaders/debug-line-2d.glsl");

        private static BufferObject<float> _vertexBufferObject;
        private static VertexArrayObject<float> _vertexArrayobject;

        private static bool _startedOnGpu;

        public static void Start()
        {
            _vertexBufferObject =
                new BufferObject<float>(VertexArray, BufferTarget.ArrayBuffer, BufferUsageHint.DynamicDraw);
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
            Shader.Dispose();
        }

        public static void BeginFrame()
        {
            if (!_startedOnGpu)
            {
                Start();
                _startedOnGpu = true;
            }

            // Remove dead lines
            for (var i = 0; i < Line2Ds.Count; i++)
            {
                Line2D line = Line2Ds[i];
                if (line.BeginFrame() < 0)
                {
                    Line2Ds.Remove(line);
                    i--;
                }
            }
        }

        public static void Draw()
        {
            if (!Line2Ds.Any())
            {
                return;
            }

            var index = 0;
            foreach (Line2D line in Line2Ds)
            {
                // 2 is verticies
                for (var i = 0; i < 2; i++)
                {
                    Vector2 point = i is 0 ? line.From : line.To;
                    Vector3 color = line.Color;

                    VertexArray[index] = point.X;
                    VertexArray[index + 1] = point.Y;
                    VertexArray[index + 2] = 0.0f;

                    VertexArray[index + 3] = color.X;
                    VertexArray[index + 4] = color.Y;
                    VertexArray[index + 5] = color.Z;

                    index += 6;
                }
            }

            int subDataSize = Line2Ds.Count * 6 * 2;
            var subData = new float[subDataSize];
            Array.Copy(VertexArray, subData, subDataSize);
            _vertexBufferObject.Bind();
            _vertexBufferObject.ReBufferData(subData);

            Shader.Use();
            Shader.SetMatrix4("uProjection", SceneManager.CurrentScene.Camera2D.ProjectionMatrix);
            Shader.SetMatrix4("uView", SceneManager.CurrentScene.Camera2D.GetViewMatrix());

            _vertexArrayobject.Bind();
            _vertexArrayobject.EnableVertexAttribPointers();

            GL.DrawElements(PrimitiveType.Lines, subDataSize, DrawElementsType.UnsignedInt, IntPtr.Zero);

            _vertexArrayobject.DisableVertexAttribPointers();
            _vertexArrayobject.Unbind();

            Shader.Detach();
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
            if (Line2Ds.Count >= MaxLines)
            {
                return;
            }

            Line2Ds.Add(new Line2D(from, to, color, lifeTime));
        }
    }
}