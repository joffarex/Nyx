using Nyx.Core;
using Nyx.Core.Settings;

var windowSettings = new WindowSettings(1920, 1080, "Nyx Playground");
using var window = new Window(windowSettings);
window.Run();