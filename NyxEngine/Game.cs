using System;
using System.Drawing;

namespace NyxEngine
{
    public class Game : NyxEngine
    {
        private int _frame = 0;

        public Shape2D Player;

        public Game() : base(new Vector2(690, 420), "Demo")
        {
        }

        protected override void OnLoad()
        {
            Console.WriteLine("Loaded...");
            BackgroundColor = Color.Black;

            Player = new Shape2D(new Vector2(10, 10), new Vector2(10, 10), "1");
        }


        protected override void OnUpdate()
        {
            Console.WriteLine($"Frames: {_frame}");
            _frame++;
        }

        protected override void OnDraw()
        {
        }
    }
}