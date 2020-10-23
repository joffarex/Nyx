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

        private Game()
        {
        }

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

        protected override void KeyDown(IKeyboard arg1, Key arg2, int arg3)
        {
            base.KeyDown(arg1, arg2, arg3);

            // Call key down events from game objects here in order to subscribe
            // TODO: Implement functionality to automatically add game object events here
            //
            // There must be centralized object with abstract event methods, which will get called here with looping over them
        }

        protected override void KeyUp(IKeyboard arg1, Key arg2, int arg3)
        {
            base.KeyUp(arg1, arg2, arg3);
        }

        protected override void MouseClick(IMouse arg1, MouseButton arg2)
        {
            base.MouseClick(arg1, arg2);

            // Call mouse click events from game objects here 
        }

        protected override void MouseMove(IMouse arg1, PointF arg2)
        {
            base.MouseMove(arg1, arg2);
            //Console.WriteLine($"X:{arg2.X} Y:{arg2.Y}");

            // Call mouse move events from game objects here 
        }

        protected override void MouseDown(IMouse arg1, MouseButton arg2)
        {
            base.MouseDown(arg1, arg2);
        }

        protected override void MouseUp(IMouse arg1, MouseButton arg2)
        {
            base.MouseUp(arg1, arg2);
        }

        protected override void MouseDoubleClick(IMouse arg1, MouseButton arg2)
        {
            base.MouseDoubleClick(arg1, arg2);
        }

        protected override void MouseScroll(IMouse arg1, ScrollWheel arg2)
        {
            base.MouseScroll(arg1, arg2);
        }
    }
}