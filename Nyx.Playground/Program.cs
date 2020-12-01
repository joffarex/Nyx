using Nyx.Core;
using Nyx.Core.Settings;
using Nyx.Playground;

var windowSettings = new WindowSettings(1920, 1080, "Nyx Playground");
using var window = new Window(windowSettings);
Window.AddScene("test", new TestScene());
window.Run();