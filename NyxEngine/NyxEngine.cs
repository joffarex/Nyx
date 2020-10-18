using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace NyxEngine
{
    public abstract class NyxEngine
    {
        private readonly Vector2 _screenSize = new Vector2(512, 512);
        private string _title = "New game";
        private Canvas _window;
        private Thread _gameLoopThread;

        public Color BackgroundColor = Color.Aqua;

        public NyxEngine()
        {
            CreateWindow();
        }

        public NyxEngine(Vector2 screenSize, string title)
        {
            _screenSize = screenSize;
            _title = title;

            CreateWindow();
        }

        private void CreateWindow()
        {
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
                    _window.BeginInvoke((MethodInvoker)delegate { _window.Refresh(); });
                    OnUpdate();
                    Thread.Sleep(1);
                }
                catch
                {
                    Console.WriteLine("Game is loading..");
                }
            }
        }

        private void Renderer(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(BackgroundColor);
        }

        public abstract void OnLoad();
        public abstract void OnUpdate();
        public abstract void OnDraw();
    }
}