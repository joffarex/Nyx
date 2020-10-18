
using System;
using System.Drawing;

namespace NyxEngine
{
    public class Game : NyxEngine
    {
        public Game() : base(new Vector2(690, 420), "Demo")
        {
        }

        public override void OnLoad()
        {
            Console.WriteLine("Loaded...");
            BackgroundColor = Color.BlueViolet;
        }

        private int frame = 0;
        public override void OnUpdate()
        {
            Console.WriteLine($"Frames: {frame}");
            frame++;
        }

        public override void OnDraw()
        {
        }
    }
}