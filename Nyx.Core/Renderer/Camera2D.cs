using OpenTK.Mathematics;

namespace Nyx.Core.Renderer
{
    public class Camera2D
    {
        public Camera2D(Vector2 position, Vector2 baseSize)
        {
            Position = position;
            BaseSize = baseSize;
            ProjectionMatrix = Matrix4.Identity;
            InverseProjection = new Matrix4();
            ViewMatrix = Matrix4.Identity;
            InverseView = new Matrix4();
            Front = new Vector3(0.0f, 0.0f, -1.0f);
            Up = new Vector3(0.0f, 1.0f, 0.0f);
            AdjustProjection();
        }

        public Vector2 Position { get; init; }

        public Matrix4 ProjectionMatrix { get; private set; }

        public Matrix4 InverseProjection { get; private set; }
        private Matrix4 ViewMatrix { get; set; }

        public Matrix4 InverseView { get; set; }

        public Vector3 Front { get; set; }

        public Vector3 Up { get; init; }

        public Vector2 BaseSize { get; set; }

        public void AdjustProjection()
        {
            var ortho = Matrix4.CreateOrthographicOffCenter(0.0f, BaseSize.X, 0.0f, BaseSize.Y, 0.0f, 100.0f);
            ProjectionMatrix = ortho;

            Matrix4.Invert(ProjectionMatrix, out Matrix4 inverseProjection);
            InverseProjection = inverseProjection;
        }

        public Matrix4 GetViewMatrix()
        {
            Matrix4 lookAtMatrix = Matrix4.LookAt(new Vector3(Position.X, Position.Y, 20.0f),
                Front + new Vector3(Position.X, Position.Y, 0.0f), Up);

            ViewMatrix = lookAtMatrix;

            Matrix4.Invert(ViewMatrix, out Matrix4 inverseView);
            InverseView = inverseView;

            return ViewMatrix;
        }
    }
}