using System.Drawing;
using System.Windows.Forms;

namespace NyxEngine
{
    public abstract class NyxEngine
    {
        private readonly Vector2 _screenSize = new Vector2(512, 512);
        private string _title = "New game";
        private Canvas _window;

        public NyxEngine()
        {
            CreateWindow();
        }

        public NyxEngine(Vector2 screenSize, string title)
        {
            _screenSize = screenSize;
            _title = title;

            CreateWindow();
        }

        private void CreateWindow()
        {
            _window = new Canvas();
            _window.Size = new Size((int) _screenSize.X, (int) _screenSize.Y);
            _window.Text = _title;
            Application.Run(_window);
        }
    }
}