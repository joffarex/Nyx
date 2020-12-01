using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Nyx.Core.Logger;

namespace Nyx.Core.Scenes
{
    public class SceneManager
    {
        private static readonly ILogger<SceneManager> Logger = SerilogLogger.Factory.CreateLogger<SceneManager>();

        private static Scene _currentScene;

        public static readonly Dictionary<string, Scene> Scenes = new();
        public static Scene CurrentScene => _currentScene;

        public static void AddScene(string name, Scene scene)
        {
            Scenes.Add(name, scene);
        }

        public static void ChangeScene(string name)
        {
            bool result = Scenes.TryGetValue(name, out _currentScene);

            if (!result)
            {
                Logger.LogError($"Scene '{name}' does not exist");
                return;
            }

            _currentScene.LoadResources();
            // load
            _currentScene.Init();
            _currentScene.Start();
        }
    }
}