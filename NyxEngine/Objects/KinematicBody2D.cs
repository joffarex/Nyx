using System;
using System.Windows.Forms;
using NyxEngine.Utils;

namespace NyxEngine.Objects
{
    public abstract class KinematicBody2D : Sprite2D
    {
        protected KinematicBody2D(Vector2 position, Vector2 scale, string filePath) : base(position, scale, filePath,
            nameof(KinematicBody2D))
        {
            Logger.Info($"Registering {nameof(KinematicBody2D)} - ({Tag})");
            NyxEngine.RegisterKinematicBody(this);
            NyxEngine.SubscribeKinematicBodyKeyEvents(this);
        }

        ~KinematicBody2D()
        {
            Dispose(false);
        }

        public void DestroySelf()
        {
            Logger.Info($"Unregistering {nameof(KinematicBody2D)} - ({Tag})");
            Dispose();
            NyxEngine.UnregisterKinematicBody(this);
        }

        public void InputKeyDown(object sender, KeyEventArgs e)
        {
            GetKeyDown(e);
        }

        public void InputKeyUp(object sender, KeyEventArgs e)
        {
            GetKeyUp(e);
        }

        protected abstract void GetKeyDown(KeyEventArgs e);
        protected abstract void GetKeyUp(KeyEventArgs e);


        public override void Dispose()
        {
            try
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            finally
            {
                base.Dispose(true);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (Disposed) return;

            if (disposing)
            {
                Tag = string.Empty;

                if (Position != null)
                {
                    Position.Dispose();
                    Position = null;
                }

                if (Scale != null)
                {
                    Scale.Dispose();
                    Scale = null;
                }
            }

            Disposed = true;
        }
    }
}