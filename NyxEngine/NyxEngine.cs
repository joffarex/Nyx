using System.Drawing;
using System.Windows.Forms;

namespace NyxEngine
{
    public abstract class NyxEngine
    {
        private Vector2 _screenSize = new Vector2(512,512);
        private string _title = "New game";
        private Canvas _window = null;
        
        public NyxEngine(Vector2 screenSize, string title)
        {
            _screenSize = screenSize;
            _title = title;
            
            _window = new Canvas();
            _window.Size = new Size((int)this._screenSize.X, (int)this._screenSize.Y);
            _window.Text = _title;
            
            Application.Run(_window);
        }
    }
}