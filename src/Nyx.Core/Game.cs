using System;
using System.Collections.Generic;
using System.Drawing;
using Nyx.Core.Utils;
using Silk.NET.Input.Common;

namespace Nyx.Core
{
    public class Game : NyxEngine
    {
        public static Scene CurrentScene;
        private static Game _instance;

        private readonly Dictionary<int, Scene> _scenes = new Dictionary<int, Scene>();


        private Game() : base(Width, Height)
        {
        }

        public static int Width { get; } = 800;
        public static int Height { get; } = 600;


        public static Game Get()
        {
            if (_instance == null)
            {
                _instance = new Game();
            }

            return _instance;
        }

        public void AddScene(int index, Scene scene)
        {
            _scenes.Add(index, scene);
        }


        public void ChangeScene(int newSceneIndex)
        {
            bool result = _scenes.TryGetValue(newSceneIndex, out CurrentScene);
            if (!result)
            {
                throw new Exception($"Scene idx: {newSceneIndex} does not exist");
            }

            CurrentScene.Init();
            CurrentScene.Start();
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            ChangeScene(0);
        }

        protected override void OnRender(double obj)
        {
            base.OnRender(obj);

            CurrentScene.Render();
        }


        protected override void OnUpdate(double obj)
        {
            base.OnUpdate(obj);

            PrintFps(DeltaTime);

            if (DeltaTime >= 0)
            {
                CurrentScene.Update(DeltaTime);
            }

            EndTime = Time.GetTimeFromAppicationStart();
            DeltaTime = EndTime - BeginTime;
            BeginTime = EndTime;
        }

        public static float GetFps(float deltaTime)
        {
            return 1.0f / deltaTime;
        }

        public static void PrintFps(float deltaTime)
        {
            Console.WriteLine($"{GetFps(deltaTime)}");
        }

        protected override void OnClose()
        {
            base.OnClose();

            CurrentScene.Dispose();
        }

        protected override void KeyDown(IKeyboard keyboard, Key key, int keyCode)
        {
            base.KeyDown(keyboard, key, keyCode);
        }

        protected override void KeyUp(IKeyboard keyboard, Key key, int keyCode)
        {
            base.KeyUp(keyboard, key, keyCode);
        }

        protected override void MouseClick(IMouse mouse, MouseButton mouseButton)
        {
            base.MouseClick(mouse, mouseButton);
        }

        protected override void MouseMove(IMouse mouse, PointF position)
        {
            base.MouseMove(mouse, position);
            //Console.WriteLine($"X:{arg2.X} Y:{arg2.Y}");

            CurrentScene.MouseMove(mouse, position);
        }

        protected override void MouseDown(IMouse mouse, MouseButton mouseButton)
        {
            base.MouseDown(mouse, mouseButton);
        }

        protected override void MouseUp(IMouse mouse, MouseButton mouseButton)
        {
            base.MouseUp(mouse, mouseButton);
        }

        protected override void MouseDoubleClick(IMouse mouse, MouseButton mouseButton)
        {
            base.MouseDoubleClick(mouse, mouseButton);
        }

        protected override void MouseScroll(IMouse mouse, ScrollWheel scrollWheel)
        {
            base.MouseScroll(mouse, scrollWheel);

            CurrentScene.MouseScroll(mouse, scrollWheel);
        }
    }
}