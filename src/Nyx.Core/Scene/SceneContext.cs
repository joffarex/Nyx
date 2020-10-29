using System;
using System.Collections.Generic;
using System.Numerics;

namespace Nyx.Core.Scene
{
    public static class SceneContext
    {
        public static Scene CurrentScene;
        private static readonly Dictionary<int, Scene> Scenes = new Dictionary<int, Scene>();

        public static Vector2 BaseSize { get; set; }

        public static void AddScene(int index, Scene scene)
        {
            Scenes.Add(index, scene);
        }

        public static void ChangeScene(int newSceneIndex)
        {
            bool result = Scenes.TryGetValue(newSceneIndex, out CurrentScene);
            if (!result)
            {
                throw new Exception($"Scene idx: {newSceneIndex} does not exist");
            }

            CurrentScene.LoadResources();
            CurrentScene.Load();
            CurrentScene.Init();
            CurrentScene.Start();
        }
    }
}