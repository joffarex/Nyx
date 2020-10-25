using System;
using System.Drawing;
using System.Numerics;
using Nyx.Core.Math;
using Silk.NET.Input.Common;

namespace Nyx.Core.OpenGL
{
    public class Camera3D
    {
        private float _zoom = 45f;

        public Camera3D(Vector3 position, Vector3 front, Vector3 up, float aspectRatio)
        {
            Position = position;
            AspectRatio = aspectRatio;
            Front = front;
            Up = up;
        }

        public Vector3 Position { get; set; }
        public Vector3 Front { get; set; }

        public Vector3 Up { get; }
        public float AspectRatio { get; set; }

        public float Yaw { get; set; } = -90f;
        public float Pitch { get; set; }

        public void ModifyZoom(float zoomAmount)
        {
            //We don't want to be able to zoom in too close or too far away so clamp to these values
            _zoom = System.Math.Clamp(_zoom - zoomAmount, 1.0f, 45f);
        }

        public void ModifyDirection(float xOffset, float yOffset)
        {
            Yaw += xOffset;
            Pitch -= yOffset;

            //We don't want to be able to look behind us by going over our head or under our feet so make sure it stays within these bounds
            Pitch = System.Math.Clamp(Pitch, -89f, 89f);

            Vector3 cameraDirection = Vector3.Zero;
            cameraDirection.X = MathF.Cos(Converters.DegreesToRadians(Yaw)) *
                                MathF.Cos(Converters.DegreesToRadians(Pitch));
            cameraDirection.Y = MathF.Sin(Converters.DegreesToRadians(Pitch));
            cameraDirection.Z = MathF.Sin(Converters.DegreesToRadians(Yaw)) *
                                MathF.Cos(Converters.DegreesToRadians(Pitch));

            Front = Vector3.Normalize(cameraDirection);
        }

        public Matrix4x4 GetViewMatrix()
        {
            return Matrix4x4.CreateLookAt(Position, Position + Front, Up);
        }

        public Matrix4x4 GetProjectionMatrix()
        {
            return Matrix4x4.CreatePerspectiveFieldOfView(Converters.DegreesToRadians(_zoom), AspectRatio, 0.1f,
                100.0f);
        }

        public void Update(float deltaTime)
        {
            float moveSpeed = 2.5f * deltaTime;

            if (NyxEngine.Input.IsKeyPressed(Key.W))
            {
                Position += moveSpeed * Front;
            }

            if (NyxEngine.Input.IsKeyPressed(Key.S))
            {
                Position -= moveSpeed * Front;
            }

            if (NyxEngine.Input.IsKeyPressed(Key.A))
            {
                Position -= Vector3.Normalize(Vector3.Cross(Front, Up)) * moveSpeed;
            }

            if (NyxEngine.Input.IsKeyPressed(Key.D))
            {
                Position += Vector3.Normalize(Vector3.Cross(Front, Up)) * moveSpeed;
            }
        }

        public void MouseMove(PointF lastMousePosition, PointF position)
        {
            const float lookSensitivity = 0.1f;
            if (lastMousePosition == default)
            {
                lastMousePosition = position;
            }
            else
            {
                float xOffset = (position.X - lastMousePosition.X) * lookSensitivity;
                float yOffset = (position.Y - lastMousePosition.Y) * lookSensitivity;
                lastMousePosition = position;

                ModifyDirection(xOffset, yOffset);
            }
        }
    }
}