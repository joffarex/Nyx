using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using NyxEngine.Old.Objects;
using NyxEngine.Old.Utils;

namespace NyxEngine.Old
{
    public abstract class NyxEngine
    {
        private static readonly List<Shape2D> Shapes = new List<Shape2D>();
        private static readonly List<Sprite2D> Sprites = new List<Sprite2D>();
        private static readonly List<KinematicBody2D> KinematicBodies = new List<KinematicBody2D>();
        private static Canvas _window;
        private readonly Vector2 _screenSize = new Vector2(512, 512);
        private readonly string _title = "New game";
        private Thread _gameLoopThread;

        protected Color BackgroundColor = Color.Aqua;
        public float CameraAngle = 0f;

        public Vector2 CameraPosition = Vector2.Zero();

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
            _window.KeyDown += InputKeyDown;
            _window.KeyUp += InputKeyUp;

            _gameLoopThread = new Thread(GameLoop);
            _gameLoopThread.Start();

            Application.Run(_window);
        }

        private void GameLoop()
        {
            try
            {
                OnLoad();
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                if (!string.IsNullOrEmpty(e.StackTrace)) Logger.Error($"[STACK TRACE] {e.StackTrace}");
            }

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

            // Camera
            g.TranslateTransform(CameraPosition.X, CameraPosition.Y);
            g.RotateTransform(CameraAngle);

            foreach (var shape in Shapes)
                g.FillRectangle(new SolidBrush(Color.Red), shape.Position.X, shape.Position.Y, shape.Scale.X,
                    shape.Scale.Y);

            foreach (var sprite in Sprites)
                g.DrawImage(sprite.Sprite, sprite.Position.X, sprite.Position.Y, sprite.Scale.X, sprite.Scale.Y);

            foreach (var kinematicBody in KinematicBodies)
                g.DrawImage(kinematicBody.Sprite, kinematicBody.Position.X, kinematicBody.Position.Y,
                    kinematicBody.Scale.X, kinematicBody.Scale.Y);
        }

        public static void SubscribeKinematicBodyKeyEvents(KinematicBody2D kinematicBody)
        {
            _window.KeyDown += kinematicBody.InputKeyDown;
            _window.KeyUp += kinematicBody.InputKeyUp;
        }

        private void InputKeyUp(object sender, KeyEventArgs e)
        {
            GetKeyUp(e);
        }

        private void InputKeyDown(object sender, KeyEventArgs e)
        {
            GetKeyDown(e);
        }

        protected abstract void OnLoad();
        protected abstract void OnUpdate();
        protected abstract void OnDraw();
        protected abstract void GetKeyDown(KeyEventArgs e);
        protected abstract void GetKeyUp(KeyEventArgs e);

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

        public static void RegisterKinematicBody(KinematicBody2D kinematicBody)
        {
            KinematicBodies.Add(kinematicBody);
        }

        public static void UnregisterKinematicBody(KinematicBody2D kinematicBody)
        {
            KinematicBodies.Remove(kinematicBody);
        }
    }
}