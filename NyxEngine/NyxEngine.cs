using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using NyxEngine.Utils;

namespace NyxEngine
{
    public abstract class NyxEngine
    {
        private readonly Vector2 _screenSize = new Vector2(512, 512);
        private string _title = "New game";
        private Canvas _window;
        private Thread _gameLoopThread;
        private static readonly List<Shape2D> Shapes = new List<Shape2D>();

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
            Logger.Info("Game is starting");
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
            {
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
        }

        private void Renderer(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            // Change background color
            g.Clear(BackgroundColor);

            foreach (var shape in Shapes)
            {
                g.FillRectangle(new SolidBrush(Color.Red), shape.Position.X, shape.Position.Y, shape.Scale.X,
                    shape.Scale.Y);
            }
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
    }
}