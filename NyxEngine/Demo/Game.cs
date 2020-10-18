using System;
using System.Drawing;
using NyxEngine.Objects;

namespace NyxEngine.Demo
{
    public class Game : NyxEngine
    {
        public static readonly Vector2 ScreenSize = new Vector2(690, 420);

        public string[,] Map =
        {
            {".", ".", ".", ".", "#", "#"},
            {".", ".", ".", ".", ".", "."},
            {".", ".", "#", ".", ".", "."},
            {".", ".", "#", ".", "#", "."},
            {".", "#", "#", "#", "#", "#"},
            {".", ".", ".", "#", ".", "."}
        };

        public Sprite2D Player;

        public Game() : base(ScreenSize, "Demo")
        {
        }

        protected override void OnLoad()
        {
            BackgroundColor = Color.Black;

            //Player = new Shape2D(new Vector2(10, 10), new Vector2(10, 10), Guid.NewGuid().ToString());
            //Player = new Sprite2D(new Vector2(10, 10), new Vector2(20, 20), $"{nameof(Player)}.png");

            var (scaleX, scaleY) =
                new Tuple<float, float>(ScreenSize.X / Map.GetLength(0), ScreenSize.Y / Map.GetLength(1));
            
            for (var i = 0; i < Map.GetLength(0); i++)
            for (var j = 0; j < Map.GetLength(1); j++)
                if (Map[j, i] == "#")
                {
                    var sprite = new Sprite2D(new Vector2(i * 40, j * 40), new Vector2(40, 40),
                        "Block.png");
                }
        }

        protected override void OnUpdate()
        {
        }

        protected override void OnDraw()
        {
        }
    }
}