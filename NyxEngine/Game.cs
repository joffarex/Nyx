
using System;
using System.Drawing;

namespace NyxEngine
{
    public class Game : NyxEngine
    {
        public Game() : base(new Vector2(690, 420), "Demo")
        {
        }

        protected override void OnLoad()
        {
            Console.WriteLine("Loaded...");
            BackgroundColor = Color.BlueViolet;
        }

        private int frame = 0;

        protected override void OnUpdate()
        {
            Console.WriteLine($"Frames: {frame}");
            frame++;
        }

        protected override void OnDraw()
        {
        }
    }
}