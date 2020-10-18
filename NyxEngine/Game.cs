using System;
using System.Drawing;
using NyxEngine.Utils;

namespace NyxEngine
{
    public class Game : NyxEngine
    {
        public Shape2D Player;

        public Game() : base(new Vector2(690, 420), "Demo")
        {
        }

        protected override void OnLoad()
        {
            BackgroundColor = Color.Black;

            Player = new Shape2D(new Vector2(10, 10), new Vector2(10, 10), Guid.NewGuid().ToString());
        }


        protected override void OnUpdate()
        {
        }

        protected override void OnDraw()
        {
        }
    }
}