using System.Drawing;
using NyxEngine.Objects;

namespace NyxEngine.Demo
{
    public class Game : NyxEngine
    {
        public Sprite2D Player;

        public Game() : base(new Vector2(690, 420), "Demo")
        {
        }

        protected override void OnLoad()
        {
            BackgroundColor = Color.Black;

            //Player = new Shape2D(new Vector2(10, 10), new Vector2(10, 10), Guid.NewGuid().ToString());
            Player = new Sprite2D(new Vector2(10, 10), new Vector2(20, 20), $"{nameof(Player)}.png");
        }

        protected override void OnUpdate()
        {
        }

        protected override void OnDraw()
        {
        }
    }
}