using System;
using System.Drawing;
using System.IO;
using NyxEngine.Old.Utils;

namespace NyxEngine.Old.Objects
{
    public class Sprite2D : Disposable
    {
        public Sprite2D(Vector2 position, Vector2 scale, string filePath, string parent)
        {
            InitializeSprite2D(position, scale, filePath, parent);
        }

        public Sprite2D(Vector2 position, Vector2 scale, string filePath)
        {
            InitializeSprite2D(position, scale, filePath, nameof(Sprite2D));

            Logger.Info($"Registering {nameof(Shape2D)} - ({Tag})");
            NyxEngine.RegisterSprite(this);
        }

        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; }
        public string Tag { get; protected set; }
        public string FilePath { get; set; }
        public Bitmap Sprite { get; set; }

        private void InitializeSprite2D(Vector2 position, Vector2 scale, string filePath, string parent)
        {
            if (!IsFilePathValid(filePath))
            {
                Dispose();
                throw new Exception("Sprite must be of type: \"png\", \"jpg\" or \"jpeg\"");
            }

            Position = position;
            Scale = scale;
            Tag = $"{parent}:{Guid.NewGuid().ToString()}";
            FilePath = filePath;
            LoadAsset();
        }

        ~Sprite2D()
        {
            Dispose(false);
        }

        private void LoadAsset()
        {
            // Triple parent is necessary as we are having assets folder outside .net project
            var baseDir = Directory.GetParent(Environment.CurrentDirectory).Parent!.Parent!.Parent!.FullName;
            var path = Path.Combine(baseDir, $"Assets/Sprites/{FilePath}");
            Sprite = new Bitmap(Image.FromFile(path), (int) Scale.X, (int) Scale.Y);
        }

        private static bool IsFilePathValid(string filePath)
        {
            var filePathParts = filePath.Split(".");
            var fileType = filePathParts[filePathParts.Length - 1];
            return fileType.Equals("png") || fileType.Equals("jpg") || fileType.Equals("jpeg");
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