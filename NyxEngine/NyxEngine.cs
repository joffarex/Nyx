using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using NyxEngine.Objects;
using NyxEngine.Utils;

namespace NyxEngine
{
    public abstract class NyxEngine
    {
        private static readonly List<Shape2D> Shapes = new List<Shape2D>();
        private static readonly List<Sprite2D> Sprites = new List<Sprite2D>();
        private readonly Vector2 _screenSize = new Vector2(512, 512);
        private readonly string _title = "New game";
        private Thread _gameLoopThread;
        private Canvas _window;

        protected Color BackgroundColor = Color.Aqua;

        protected NyxEngine()
        {
            CreateWindow();
        }

        protected NyxEngine(Vector2 screenSize, string title)
        {
            _screenSize = screenSize;
            _title = title;

            CreateWindow();
        }

        private void CreateWindow()
        {
            Logger.Info("Game is starting...");
            _window = new Canvas();
            _window.Size = new Size((int) _screenSize.X, (int) _screenSize.Y);
            _window.Text = _title;
            _window.Paint += Renderer;

            _gameLoopThread = new Thread(GameLoop);
            _gameLoopThread.Start();

            Application.Run(_window);
        }

        private void GameLoop()
        {
            OnLoad();

            while (_gameLoopThread.IsAlive)
                try
                {
                    OnDraw();
                    _window.BeginInvoke((MethodInvoker) delegate { _window.Refresh(); });
                    OnUpdate();
                    Thread.Sleep(1);
                }
                catch
                {
                    Logger.Error("Game has not been found...");
                }
        }

        private void Renderer(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            // Change background color
            g.Clear(BackgroundColor);

            foreach (var shape in Shapes)
                g.FillRectangle(new SolidBrush(Color.Red), shape.Position.X, shape.Position.Y, shape.Scale.X,
                    shape.Scale.Y);

            foreach (var sprite in Sprites)
                g.DrawImage(sprite.Sprite, sprite.Position.X, sprite.Position.Y, sprite.Scale.X, sprite.Scale.Y);
        }

        protected abstract void OnLoad();
        protected abstract void OnUpdate();
        protected abstract void OnDraw();

        public static void RegisterShape(Shape2D shape)
        {
            Shapes.Add(shape);
        }

        public static void UnregisterShape(Shape2D shape)
        {
            Shapes.Remove(shape);
        }

        public static void RegisterSprite(Sprite2D sprite)
        {
            Sprites.Add(sprite);
        }

        public static void UnregisterSprite(Sprite2D sprite)
        {
            Sprites.Remove(sprite);
        }
    }
}