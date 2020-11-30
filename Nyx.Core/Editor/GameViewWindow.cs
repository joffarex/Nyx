using System;
using System.Numerics;
using ImGuiNET;
using Microsoft.Extensions.Logging;
using Nyx.Core.Logger;

namespace Nyx.Core.Editor
{
    public class GameViewWindow
    {
        private static readonly ILogger<GameViewWindow> Logger = SerilogLogger.Factory.CreateLogger<GameViewWindow>();

        public static void ImGui()
        {
            ImGuiNET.ImGui.Begin("Game Viewport", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse);

            Vector2 windowSize = GetLargestSizeForViewport();
            Vector2 windowPos = GetCenteredPosForViewport(windowSize);

            ImGuiNET.ImGui.SetCursorPos(new Vector2(windowPos.X, windowPos.Y));

            int textureId = Window.Framebuffer.Texture.Handle;
            ImGuiNET.ImGui.Image((IntPtr) textureId, windowSize, Vector2.Zero, Vector2.UnitX);

            ImGuiNET.ImGui.End();
        }

        private static Vector2 GetLargestSizeForViewport()
        {
            Vector2 windowSize = ImGuiNET.ImGui.GetContentRegionAvail();
            windowSize.X -= ImGuiNET.ImGui.GetScrollX();
            windowSize.Y -= ImGuiNET.ImGui.GetScrollY();

            float aspectWidth = windowSize.X;
            float aspectHeight = aspectWidth / Window.WindowSettings.AspectRatio;

            if (aspectHeight > windowSize.Y)
            {
                // We must switch to pillar box mode (black bars on left and right)
                aspectHeight = windowSize.Y;
                aspectWidth = aspectHeight * Window.WindowSettings.AspectRatio;
            }

            return new Vector2(aspectWidth, aspectHeight);
        }

        private static Vector2 GetCenteredPosForViewport(Vector2 aspectSize)
        {
            Vector2 windowSize = ImGuiNET.ImGui.GetContentRegionAvail();
            windowSize.X -= ImGuiNET.ImGui.GetScrollX();
            windowSize.Y -= ImGuiNET.ImGui.GetScrollY();

            float viewportX = (windowSize.X / 2.0f) - (aspectSize.X / 2.0f);
            float viewportY = (windowSize.Y / 2.0f) - (aspectSize.Y / 2.0f);

            return new Vector2(viewportX + ImGuiNET.ImGui.GetCursorPosX(), viewportY + ImGuiNET.ImGui.GetCursorPosY());
        }
    }
}