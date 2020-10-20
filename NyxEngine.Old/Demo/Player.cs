using System.Windows.Forms;
using NyxEngine.Old.Objects;

namespace NyxEngine.Old.Demo
{
    public class Player : KinematicBody2D
    {
        public Vector2 Direction = Vector2.Zero();
        public float Speed = 2.0f;

        public Player(Vector2 position, Vector2 scale, string filePath) : base(position, scale, filePath)
        {
        }

        protected override void GetKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                Direction.Y = -1;
            }

            if (e.KeyCode == Keys.S)
            {
                Direction.Y = 1;
            }

            if (e.KeyCode == Keys.A)
            {
                Direction.X = -1;
            }

            if (e.KeyCode == Keys.D)
            {
                Direction.X = 1;
            }

            Position.X += Direction.X * Speed;
            Position.Y += Direction.Y * Speed;
        }

        protected override void GetKeyUp(KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.W) || (e.KeyCode == Keys.S))
            {
                Direction.Y = 0;
            }

            if ((e.KeyCode == Keys.A) || (e.KeyCode == Keys.D))
            {
                Direction.X = 0;
            }
        }
    }
}