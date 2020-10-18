using System;
using NyxEngine.Utils;

namespace NyxEngine.Objects
{
    public class Shape2D : Disposable
    {
        public Shape2D(Vector2 position, Vector2 scale, string tag)
        {
            Position = position;
            Scale = scale;
            Tag = tag;

            Logger.Info($"Registering {nameof(Shape2D)} - ({Tag})");
            NyxEngine.RegisterShape(this);
        }

        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; }
        public string Tag { get; private set; }

        ~Shape2D()
        {
            Dispose(false);
        }

        public void DestroySelf()
        {
            Logger.Info($"Unregistering {nameof(Shape2D)} - ({Tag})");
            Dispose();
            NyxEngine.UnregisterShape(this);
        }

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