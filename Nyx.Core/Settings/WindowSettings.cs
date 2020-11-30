using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Nyx.Core.Settings
{
    public class WindowSettings
    {
        public WindowSettings(int width, int height, string title)
        {
            Size = new Vector2i(width, height);
            Title = title;
        }

        public Vector2i Size { get; }
        public string Title { get; }

        public NativeWindowSettings MapToNativeWindowSettings()
        {
            return new()
            {
                Title = Title,
                Size = Size,
                WindowState = WindowState.Fullscreen,
            };
        }
    }
}