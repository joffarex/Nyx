namespace Nyx.Playground
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Open the game
            Game game = Game.Get();
            game.Run();
        }
    }
}