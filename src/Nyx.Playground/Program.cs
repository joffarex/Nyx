using Nyx.Core;

namespace Nyx.Playground
{
    public class Program
    {
        public static void Main(string[] args)
        {
            NyxApp app = NyxApp.Get(1600, 900, "Nyx Playground");
            app.AddScene(0, new LevelEditorScene());
            app.AddScene(1, new LevelScene());

            app.Run();
        }
    }
}