using System;
using System.Drawing;
using System.Numerics;
using Nyx.Core.Math;
using Silk.NET.Input.Common;
using static Nyx.Core.Event.EventContext;

namespace Nyx.Core.Renderer
{
    public class Camera3D
    {
        private float _zoom = 45f;

        public Camera3D(float aspectRatio)
        {
            Init(Vector3.UnitZ * 6, Vector3.UnitZ * -1, Vector3.UnitY, aspectRatio);
        }

        public Camera3D(Vector3 position, Vector3 front, Vector3 up, float aspectRatio)
        {
            Init(position, front, up, aspectRatio);
        }

        public Vector3 Position { get; set; }
        public Vector3 Front { get; set; }

        public Vector3 Up { get; private set; }
        public float AspectRatio { get; set; }

        public float Yaw { get; set; } = -90f;
        public float Pitch { get; set; }

        private void Init(Vector3 position, Vector3 front, Vector3 up, float aspectRatio)
        {
            Position = position;
            Front = front;
            Up = up;
            AspectRatio = aspectRatio;
        }

        public void MouseScroll(float zoomAmount)
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

            if (KeyEvent.IsKeyPressed(Key.W))
            {
                Position += moveSpeed * Front;
            }

            if (KeyEvent.IsKeyPressed(Key.S))
            {
                Position -= moveSpeed * Front;
            }

            if (KeyEvent.IsKeyPressed(Key.A))
            {
                Position -= Vector3.Normalize(Vector3.Cross(Front, Up)) * moveSpeed;
            }

            if (KeyEvent.IsKeyPressed(Key.D))
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