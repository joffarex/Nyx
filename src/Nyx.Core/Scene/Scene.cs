using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Nyx.Core.Renderer;
using Nyx.Core.Serializers;
using Nyx.Ecs;
using Nyx.Utils;
using Silk.NET.Input.Common;

namespace Nyx.Core.Scene
{
    public abstract class Scene : IDisposable
    {
        protected readonly BatchRenderer BatchRenderer = new BatchRenderer();

        // protected static PointF LastMousePosition;
        protected readonly List<GameObject> GameObjects = new List<GameObject>();

        private bool _isRunning;

        protected GameObject ActiveGameObject;

        protected bool LevelLoaded;

        // public Camera3D Camera3D { get; protected set; }
        public Camera2D Camera2D { get; protected set; }

        public virtual void Dispose()
        {
            BatchRenderer.Dispose();

            foreach (GameObject gameObject in GameObjects)
            {
                gameObject.Dispose();
            }
        }

        public abstract void Init();

        public void Start()
        {
            foreach (GameObject gameObject in GameObjects)
            {
                gameObject.Start();
                BatchRenderer.Add(gameObject);
            }

            _isRunning = true;
        }

        public void AddGameObjectToScene(GameObject gameObject)
        {
            if (!_isRunning)
            {
                GameObjects.Add(gameObject);
            }
            else
            {
                GameObjects.Add(gameObject);
                gameObject.Start();
                BatchRenderer.Add(gameObject);
            }
        }

        public virtual void Update(float deltaTime)
        {
            foreach (GameObject gameObject in GameObjects)
            {
                gameObject.Update(deltaTime);
            }

            BatchRenderer.Update(deltaTime);
        }

        public virtual void MouseMove(IMouse mouse, PointF position)
        {
        }

        public virtual void MouseScroll(IMouse mouse, ScrollWheel scrollWheel)
        {
        }

        public virtual void Render()
        {
            foreach (GameObject gameObject in GameObjects)
            {
                gameObject.Render();
            }
        }

        public void SceneImGui()
        {
            if (ActiveGameObject != null)
            {
                ImGuiNET.ImGui.Begin("Inspector");
                ActiveGameObject.ImGui();
                ImGuiNET.ImGui.End();
            }

            ImGui();
        }

        public virtual void ImGui()
        {
        }

        public virtual void LoadResources()
        {
        }

        public void SaveExit()
        {
            string json = JsonConvert.SerializeObject(GameObjects, Formatting.Indented, new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Include,
                NullValueHandling = NullValueHandling.Include,

                Converters = new List<JsonConverter> {new ComponentConverter()},
            });

            using (var writer = new StreamWriter(PathUtils.GetFullPath("levels/levelEditor.json")))
            {
                writer.Write(json);
            }
        }

        public void Load()
        {
            var src = "";

            try
            {
                src = File.ReadAllText(PathUtils.GetFullPath("levels/levelEditor.json"));
            }
            catch (FileNotFoundException)
            {
            }

            if (string.IsNullOrEmpty(src))
            {
                return;
            }

            int maxGameObjectId = -1;
            int maxComponentId = -1;

            var gameObjects = JsonConvert.DeserializeObject<List<GameObject>>(src, new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Include,
                NullValueHandling = NullValueHandling.Include,

                Converters = new List<JsonConverter> {new GameObjectConverter()},
            });
            foreach (GameObject gameObject in gameObjects)
            {
                AddGameObjectToScene(gameObject);

                maxComponentId = gameObject.Components.Select(component => component.Uid).Prepend(maxComponentId).Max();

                if (gameObject.Uid > maxGameObjectId)
                {
                    maxGameObjectId = gameObject.Uid;
                }
            }


            // We need to make sure that these are higher than an actual max id
            // that way we dont get duplicates
            maxGameObjectId++;
            maxComponentId++;
            GameObject.Init(maxGameObjectId);
            Component.Init(maxComponentId);
            LevelLoaded = true;
        }
    }
}