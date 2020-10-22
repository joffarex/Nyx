using System;
using Nyx.Engine;
using Silk.NET.Input.Common;

namespace Nyx.Playground
{
    public class LevelEditorScene : Scene
    {
        private bool _changingScene;
        private float _timeToChangeScene = 2.0f;

        public LevelEditorScene()
        {
            Console.WriteLine("LevelEditorScene");
        }

        public override void Update(float deltaTime)
        {
            if (!_changingScene && NyxEngine.KeyListener.IsKeyPressed(Key.Space))
            {
                _changingScene = true;
            }

            if (_changingScene && (_timeToChangeScene > 0))
            {
                _timeToChangeScene -= deltaTime;
                Game.Get().r -= deltaTime * 5.0f;
                Game.Get().g -= deltaTime * 5.0f;
                Game.Get().b -= deltaTime * 5.0f;
            }
            else if (_changingScene)
            {
                Game.ChangeScene(1);
                _changingScene = false;
            }
        }
    }
}