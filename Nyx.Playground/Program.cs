using Nyx.Core;
using Nyx.Core.Settings;
using Nyx.Playground;
using OpenTK.Mathematics;

var windowSettings = new WindowSettings(1920, 1080, "Nyx Playground");
using var window = new Window(windowSettings);
Window.AddBaseSize(new Vector2(32.0f * 40.0f, 32.0f * 21.0f));
Window.AddScene("test", new TestScene());
window.Run();