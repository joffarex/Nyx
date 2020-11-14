using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;
using ImGuiNET;
using Nyx.Core.Utils;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Vector2 = System.Numerics.Vector2;
using Vector4 = System.Numerics.Vector4;

namespace Nyx.Core.Gui
{
    public class ImGuiController : IDisposable
    {
        private readonly List<char> _pressedChars = new();

        private readonly Vector2 _scaleFactor = Vector2.One;

        private GuiTexture _fontTexture;
        private bool _frameBegun;
        private int _indexBuffer;
        private int _indexBufferSize;

        private GuiShader _shader;

        private int _vertexArray;
        private int _vertexBuffer;
        private int _vertexBufferSize;
        private int _windowHeight;

        private int _windowWidth;

        public ImGuiController(Vector2i size)
        {
            (int x, int y) = size;
            _windowWidth = x;
            _windowHeight = y;

            IntPtr context = ImGui.CreateContext();
            ImGui.SetCurrentContext(context);
            ImGuiIOPtr io = ImGui.GetIO();

            io.WantSaveIniSettings = true;

            var fontConfig = new ImFontConfigPtr();

            string fontPath = PathUtils.GetFullPath("assets/fonts/segoeui.ttf");
            io.Fonts.AddFontFromFileTTF(fontPath, 20, fontConfig);

            io.BackendFlags |= ImGuiBackendFlags.RendererHasVtxOffset;

            CreateDeviceResources();
            SetKeyMappings();

            SetPerFrameImGuiData(1f / 60f);

            ImGui.NewFrame();
            _frameBegun = true;
        }

        public void Dispose()
        {
            _fontTexture.Dispose();
            _shader.Dispose();
        }


        public void WindowResized(Vector2i size)
        {
            (int x, int y) = size;
            _windowWidth = x;
            _windowHeight = y;
        }

        public void DestroyDeviceObjects()
        {
            Dispose();
        }

        private void CreateDeviceResources()
        {
            Utils.CreateVertexArray("ImGui", out _vertexArray);

            _vertexBufferSize = 10000;
            _indexBufferSize = 2000;

            Utils.CreateVertexBuffer("ImGui", out _vertexBuffer);
            Utils.CreateElementBuffer("ImGui", out _indexBuffer);
            GL.NamedBufferData(_vertexBuffer, _vertexBufferSize, IntPtr.Zero, BufferUsageHint.DynamicDraw);
            GL.NamedBufferData(_indexBuffer, _indexBufferSize, IntPtr.Zero, BufferUsageHint.DynamicDraw);

            RecreateFontDeviceTexture();

            const string vertexSource = @"#version 330 core

uniform mat4 projection_matrix;

layout(location = 0) in vec2 in_position;
layout(location = 1) in vec2 in_texCoord;
layout(location = 2) in vec4 in_color;

out vec4 color;
out vec2 texCoord;

void main()
{
    gl_Position = projection_matrix * vec4(in_position, 0, 1);
    color = in_color;
    texCoord = in_texCoord;
}";
            const string fragmentSource = @"#version 330 core

uniform sampler2D in_fontTexture;

in vec4 color;
in vec2 texCoord;

out vec4 outputColor;

void main()
{
    outputColor = color * texture(in_fontTexture, texCoord);
}";

            _shader = new GuiShader("ImGui", vertexSource, fragmentSource);

            GL.VertexArrayVertexBuffer(_vertexArray, 0, _vertexBuffer, IntPtr.Zero, Unsafe.SizeOf<ImDrawVert>());
            GL.VertexArrayElementBuffer(_vertexArray, _indexBuffer);

            GL.EnableVertexArrayAttrib(_vertexArray, 0);
            GL.VertexArrayAttribBinding(_vertexArray, 0, 0);
            GL.VertexArrayAttribFormat(_vertexArray, 0, 2, VertexAttribType.Float, false, 0);

            GL.EnableVertexArrayAttrib(_vertexArray, 1);
            GL.VertexArrayAttribBinding(_vertexArray, 1, 0);
            GL.VertexArrayAttribFormat(_vertexArray, 1, 2, VertexAttribType.Float, false, 8);

            GL.EnableVertexArrayAttrib(_vertexArray, 2);
            GL.VertexArrayAttribBinding(_vertexArray, 2, 0);
            GL.VertexArrayAttribFormat(_vertexArray, 2, 4, VertexAttribType.UnsignedByte, true, 16);

            Utils.CheckGlError("End of ImGui setup");
        }

        private void RecreateFontDeviceTexture()
        {
            ImGuiIOPtr io = ImGui.GetIO();
            io.Fonts.GetTexDataAsRGBA32(out IntPtr pixels, out int width, out int height, out int _);

            _fontTexture = new GuiTexture("ImGui Text Atlas", width, height, pixels);
            _fontTexture.SetMagFilter(TextureMagFilter.Linear);
            _fontTexture.SetMinFilter(TextureMinFilter.Linear);

            io.Fonts.SetTexID((IntPtr) _fontTexture.GlTexture);

            io.Fonts.ClearTexData();
        }

        public void Render()
        {
            if (_frameBegun)
            {
                _frameBegun = false;
                ImGui.Render();
                RenderImDrawData(ImGui.GetDrawData());
            }
        }

        public void Update(GameWindow wnd, float deltaSeconds)
        {
            if (_frameBegun)
            {
                ImGui.Render();
            }

            SetPerFrameImGuiData(deltaSeconds);
            UpdateImGuiInput(wnd);

            _frameBegun = true;
            ImGui.NewFrame();
        }

        private void SetPerFrameImGuiData(float deltaSeconds)
        {
            ImGuiIOPtr io = ImGui.GetIO();
            io.DisplaySize = new Vector2(
                _windowWidth / _scaleFactor.X,
                _windowHeight / _scaleFactor.Y);
            io.DisplayFramebufferScale = _scaleFactor;
            io.DeltaTime = deltaSeconds;
        }

        private void UpdateImGuiInput(GameWindow wnd)
        {
            ImGuiIOPtr io = ImGui.GetIO();

            MouseState mouseState = wnd.MouseState;
            KeyboardState keyboardState = wnd.KeyboardState;

            io.MouseDown[0] = mouseState[MouseButton.Left];
            io.MouseDown[1] = mouseState[MouseButton.Right];
            io.MouseDown[2] = mouseState[MouseButton.Middle];

            var point = new Point((int) mouseState.Position.X, (int) mouseState.Position.Y);
            io.MousePos = new Vector2(point.X, point.Y);

            OpenTK.Mathematics.Vector2 wheel = mouseState.Scroll;
            io.MouseWheel = wheel.Y;
            io.MouseWheelH = wheel.X;

            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                if (key is Keys.Unknown)
                {
                    continue;
                }

                io.KeysDown[(int) key] = keyboardState.IsKeyDown(key);
            }

            foreach (char c in _pressedChars)
            {
                io.AddInputCharacter(c);
            }

            _pressedChars.Clear();

            io.KeyCtrl = keyboardState.IsKeyDown(Keys.LeftControl) || keyboardState.IsKeyDown(Keys.RightControl);
            io.KeyAlt = keyboardState.IsKeyDown(Keys.LeftAlt) || keyboardState.IsKeyDown(Keys.RightAlt);
            io.KeyShift = keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift);
            io.KeySuper = keyboardState.IsKeyDown(Keys.LeftSuper) || keyboardState.IsKeyDown(Keys.RightSuper);
        }


        internal void PressChar(char keyChar)
        {
            _pressedChars.Add(keyChar);
        }

        private static void SetKeyMappings()
        {
            ImGuiIOPtr io = ImGui.GetIO();
            io.KeyMap[(int) ImGuiKey.Tab] = (int) Keys.Tab;
            io.KeyMap[(int) ImGuiKey.LeftArrow] = (int) Keys.Left;
            io.KeyMap[(int) ImGuiKey.RightArrow] = (int) Keys.Right;
            io.KeyMap[(int) ImGuiKey.UpArrow] = (int) Keys.Up;
            io.KeyMap[(int) ImGuiKey.DownArrow] = (int) Keys.Down;
            io.KeyMap[(int) ImGuiKey.PageUp] = (int) Keys.PageUp;
            io.KeyMap[(int) ImGuiKey.PageDown] = (int) Keys.PageDown;
            io.KeyMap[(int) ImGuiKey.Home] = (int) Keys.Home;
            io.KeyMap[(int) ImGuiKey.End] = (int) Keys.End;
            io.KeyMap[(int) ImGuiKey.Delete] = (int) Keys.Delete;
            io.KeyMap[(int) ImGuiKey.Backspace] = (int) Keys.Backspace;
            io.KeyMap[(int) ImGuiKey.Enter] = (int) Keys.Enter;
            io.KeyMap[(int) ImGuiKey.Escape] = (int) Keys.Escape;
            io.KeyMap[(int) ImGuiKey.A] = (int) Keys.A;
            io.KeyMap[(int) ImGuiKey.C] = (int) Keys.C;
            io.KeyMap[(int) ImGuiKey.V] = (int) Keys.V;
            io.KeyMap[(int) ImGuiKey.X] = (int) Keys.X;
            io.KeyMap[(int) ImGuiKey.Y] = (int) Keys.Y;
            io.KeyMap[(int) ImGuiKey.Z] = (int) Keys.Z;
        }

        private unsafe void RenderImDrawData(ImDrawDataPtr drawData)
        {
            var vertexOffsetInVertices = 0;
            var indexOffsetInElements = 0;

            if (drawData.CmdListsCount is 0)
            {
                return;
            }

            int totalVbSize = drawData.TotalVtxCount * Unsafe.SizeOf<ImDrawVert>();
            if (totalVbSize > _vertexBufferSize)
            {
                var newSize = (int) Math.Max(_vertexBufferSize * 1.5f, totalVbSize);
                GL.NamedBufferData(_vertexBuffer, newSize, IntPtr.Zero, BufferUsageHint.DynamicDraw);
                _vertexBufferSize = newSize;
            }

            int totalIbSize = drawData.TotalIdxCount * sizeof(ushort);
            if (totalIbSize > _indexBufferSize)
            {
                var newSize = (int) Math.Max(_indexBufferSize * 1.5f, totalIbSize);
                GL.NamedBufferData(_indexBuffer, newSize, IntPtr.Zero, BufferUsageHint.DynamicDraw);
                _indexBufferSize = newSize;
            }


            for (var i = 0; i < drawData.CmdListsCount; i++)
            {
                ImDrawListPtr cmdList = drawData.CmdListsRange[i];

                GL.NamedBufferSubData(_vertexBuffer, (IntPtr) (vertexOffsetInVertices * Unsafe.SizeOf<ImDrawVert>()),
                    cmdList.VtxBuffer.Size * Unsafe.SizeOf<ImDrawVert>(), cmdList.VtxBuffer.Data);
                Utils.CheckGlError($"Data Vert {i}");
                GL.NamedBufferSubData(_indexBuffer, (IntPtr) (indexOffsetInElements * sizeof(ushort)),
                    cmdList.IdxBuffer.Size * sizeof(ushort), cmdList.IdxBuffer.Data);

                Utils.CheckGlError($"Data Idx {i}");

                vertexOffsetInVertices += cmdList.VtxBuffer.Size;
                indexOffsetInElements += cmdList.IdxBuffer.Size;
            }

            ImGuiIOPtr io = ImGui.GetIO();
            var mvp = Matrix4x4.CreateOrthographicOffCenter(
                -1.0f,
                io.DisplaySize.X,
                io.DisplaySize.Y,
                0.0f,
                -1.0f,
                1.0f);

            _shader.UseShader();
            GL.UniformMatrix4(_shader.GetUniformLocation("projection_matrix"), 1, false,
                (float*) Unsafe.AsPointer(ref mvp));
            GL.Uniform1(_shader.GetUniformLocation("in_fontTexture"), 0);
            Utils.CheckGlError("Projection");

            GL.BindVertexArray(_vertexArray);
            Utils.CheckGlError("VAO");

            drawData.ScaleClipRects(io.DisplayFramebufferScale);

            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.ScissorTest);
            GL.BlendEquation(BlendEquationMode.FuncAdd);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Disable(EnableCap.CullFace);
            GL.Disable(EnableCap.DepthTest);

            // Render command lists
            var vtxOffset = 0;
            var idxOffset = 0;
            for (var n = 0; n < drawData.CmdListsCount; n++)
            {
                ImDrawListPtr cmdList = drawData.CmdListsRange[n];
                for (var cmdI = 0; cmdI < cmdList.CmdBuffer.Size; cmdI++)
                {
                    ImDrawCmdPtr pcmd = cmdList.CmdBuffer[cmdI];
                    if (pcmd.UserCallback != IntPtr.Zero)
                    {
                        throw new NotImplementedException();
                    }

                    GL.ActiveTexture(TextureUnit.Texture0);
                    GL.BindTexture(TextureTarget.Texture2D, (uint) pcmd.TextureId);
                    Utils.CheckGlError("Texture");

                    Vector4 clip = pcmd.ClipRect;
                    GL.Scissor((int) clip.X, _windowHeight - (int) clip.W, (int) (clip.Z - clip.X),
                        (int) (clip.W - clip.Y));
                    Utils.CheckGlError("Scissor");

                    GL.DrawElementsBaseVertex(PrimitiveType.Triangles, (int) pcmd.ElemCount,
                        DrawElementsType.UnsignedShort, (IntPtr) (idxOffset * sizeof(ushort)), vtxOffset);
                    Utils.CheckGlError("Draw");

                    idxOffset += (int) pcmd.ElemCount;
                }

                vtxOffset += cmdList.VtxBuffer.Size;
            }

            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.ScissorTest);
        }
    }
}