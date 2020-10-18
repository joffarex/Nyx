using System;
using System.Drawing;
using System.IO;
using NyxEngine.Utils;

namespace NyxEngine.Objects
{
    public class Sprite2D : Disposable
    {
        public Sprite2D(Vector2 position, Vector2 scale, string fileName)
        {
            Position = position;
            Scale = scale;
            Tag = Guid.NewGuid().ToString();
            FileName = fileName;

            LoadAsset();

            Logger.Info($"Registering {nameof(Shape2D)} - ({Tag})");
            NyxEngine.RegisterSprite(this);
        }

        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; }
        public string Tag { get; private set; }
        public string FileName { get; set; }
        public Bitmap Sprite { get; set; }

        ~Sprite2D()
        {
            Dispose(false);
        }

        private void LoadAsset()
        {
            // Triple parent is necessary as we are having assets folder outside .net project
            var baseDir = Directory.GetParent(Environment.CurrentDirectory).Parent!.Parent!.Parent!.FullName;
            var path = Path.Combine(baseDir, $"Assets/Sprites/{FileName}.png");
            Sprite = new Bitmap(Image.FromFile(path), (int) Scale.X, (int) Scale.Y);
        }

        public void DestroySelf()
        {
            Logger.Info($"Unregistering {nameof(Sprite2D)} - ({Tag})");
            Dispose();
            NyxEngine.UnregisterSprite(this);
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