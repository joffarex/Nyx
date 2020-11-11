using Nyx.Core;
using Nyx.Core.Settings;

var windowSettings = new WindowSettings(800, 600, "Nyx Playground");
using var window = new Window(windowSettings);
window.Run();