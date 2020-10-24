using Nyx.Core;

namespace Nyx.Playground
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Open the game
            Game game = Game.Get();
            game.AddScene(0, new LevelEditorScene());
            game.AddScene(1, new LevelScene());

            game.Run();
        }
    }
}