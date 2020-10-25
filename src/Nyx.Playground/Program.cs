using Nyx.Core;

namespace Nyx.Playground
{
    public class Program
    {
        public static void Main(string[] args)
        {
            NyxApp app = NyxApp.Get(1280, 720, "Nyx Playground");
            app.AddScene(0, new LevelEditorScene());
            app.AddScene(1, new LevelScene());

            app.Run();
        }
    }
}