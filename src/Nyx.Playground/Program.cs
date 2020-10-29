using System.Numerics;
using Nyx.Core;
using Nyx.Core.Scene;

namespace Nyx.Playground
{
    public class Program
    {
        public static void Main(string[] args)
        {
            NyxApp app = NyxApp.Get(1920, 1080, "Nyx Playground");
            app.SetBaseSize(new Vector2(32.0f * 40.0f, 32.0f * 21.0f));
            app.AddScene(0, new LevelEditorScene());
            app.AddScene(1, new LevelScene());

            app.Run();
        }
    }
}