using System;
using System.Drawing;
using Nyx.Engine;
using Silk.NET.GLFW;
using Silk.NET.Input.Common;
using MouseButton = Silk.NET.Input.Common.MouseButton;

namespace Nyx.Playground
{
    public class Game : NyxEngine
    {
        private static Scene _currentScene;
        private static Game _instance;


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


        public static void ChangeScene(int newSceneIndex)
        {
            _currentScene = newSceneIndex switch
            {
                0 => new LevelEditorScene(),
                1 => new LevelScene(),
                _ => throw new Exception($"Scene idx: {newSceneIndex} does not exist"),
            };

            _currentScene.Init();
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            ChangeScene(0);
        }

        protected override void OnRender(double obj)
        {
            base.OnRender(obj);

            _currentScene.Render();
        }


        protected override void OnUpdate(double obj)
        {
            base.OnUpdate(obj);

            if (DeltaTime >= 0)
            {
                _currentScene.Update(DeltaTime);
            }

            EndTime = (float) Glfw.GetApi().GetTime();
            DeltaTime = EndTime - BeginTime;
            BeginTime = EndTime;
        }

        public static float GetFps(float deltaTime)
        {
            return 1.0f / deltaTime;
        }

        protected override void OnClose()
        {
            base.OnClose();

            _currentScene.Dispose();
        }

        protected override void KeyDown(IKeyboard keyboard, Key key, int keyCode)
        {
            base.KeyDown(keyboard, key, keyCode);

            // Call key down events from game objects here in order to subscribe
            // TODO: Implement functionality to automatically add game object events here
            //
            // There must be centralized object with abstract event methods, which will get called here with looping over them
        }

        protected override void KeyUp(IKeyboard keyboard, Key key, int keyCode)
        {
            base.KeyUp(keyboard, key, keyCode);
        }

        protected override void MouseClick(IMouse mouse, MouseButton mouseButton)
        {
            base.MouseClick(mouse, mouseButton);

            // Call mouse click events from game objects here 
        }

        protected override void MouseMove(IMouse mouse, PointF position)
        {
            base.MouseMove(mouse, position);
            //Console.WriteLine($"X:{arg2.X} Y:{arg2.Y}");

            _currentScene.MouseMove(mouse, position);
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

            _currentScene.MouseScroll(mouse, scrollWheel);
        }
    }
}