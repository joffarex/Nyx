using System.Numerics;

namespace Nyx.Core.Renderer
{
    public class Camera2D
    {
        public Camera2D(Vector2 position)
        {
            Position = position;
            ProjectionMatrix = Matrix4x4.Identity;
            InverseProjection = new Matrix4x4();
            ViewMatrix = Matrix4x4.Identity;
            InverseView = new Matrix4x4();
            Front = new Vector3(0.0f, 0.0f, -1.0f);
            Up = new Vector3(0.0f, 1.0f, 0.0f);
            AdjustProjection();
        }

        public Vector2 Position { get; }

        public Matrix4x4 ProjectionMatrix { get; private set; }

        public Matrix4x4 InverseProjection { get; private set; }
        private Matrix4x4 ViewMatrix { get; set; }

        public Matrix4x4 InverseView { get; set; }

        public Vector3 Front { get; set; }

        public Vector3 Up { get; }

        public void AdjustProjection()
        {
            var ortho = Matrix4x4.CreateOrthographicOffCenter(0.0f, 32.0f * 40.0f, 0.0f, 32.0f * 21.0f, 0.0f, 100.0f);
            ProjectionMatrix = ortho;

            Matrix4x4 inverseProjection;
            Matrix4x4.Invert(ProjectionMatrix, out inverseProjection);
            InverseProjection = inverseProjection;
        }

        public Matrix4x4 GetViewMatrix()
        {
            var lookAtMatrix = Matrix4x4.CreateLookAt(new Vector3(Position.X, Position.Y, 20.0f),
                Front + new Vector3(Position.X, Position.Y, 0.0f), Up);

            ViewMatrix = lookAtMatrix;

            Matrix4x4 inverseView;
            Matrix4x4.Invert(ViewMatrix, out inverseView);
            InverseView = inverseView;

            return ViewMatrix;
        }
    }
}