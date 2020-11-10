using System;
using System.IO;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Silk.NET.OpenGL;

namespace Nyx.Core.Renderer
{
    public class Shader : IDisposable
    {
        private uint _handle;
        private bool _isBeingUsed;

        // TODO: add uniform caching
        // This is necessary as accessing shader for uniform locations is not performant

        public Shader(string shaderPath)
        {
            (string vertexSource, string fragmentSource) = GetShaderParts(shaderPath).GetAwaiter().GetResult();
            uint vertex = CompileShaderFromSource(ShaderType.VertexShader, vertexSource);
            uint fragment = CompileShaderFromSource(ShaderType.FragmentShader, fragmentSource);

            Init(vertex, fragment);
        }

        public Shader(string vertexPath, string fragmentPath)
        {
            uint vertex = LoadShader(ShaderType.VertexShader, vertexPath);
            uint fragment = LoadShader(ShaderType.FragmentShader, fragmentPath);

            Init(vertex, fragment);
        }

        public void Dispose()
        {
            GraphicsContext.Gl.DeleteProgram(_handle);
        }


        private void Init(uint vertex, uint fragment)
        {
            // Combine shaders under one shader program
            _handle = GraphicsContext.Gl.CreateProgram();
            GraphicsContext.Gl.AttachShader(_handle, vertex);
            GraphicsContext.Gl.AttachShader(_handle, fragment);
            LinkProgram(_handle);

            // Delete the no longer useful individual shaders;
            GraphicsContext.Gl.DetachShader(_handle, vertex);
            GraphicsContext.Gl.DetachShader(_handle, fragment);
            GraphicsContext.Gl.DeleteShader(vertex);
            GraphicsContext.Gl.DeleteShader(fragment);
        }

        public void Use()
        {
            if (!_isBeingUsed)
            {
                GraphicsContext.Gl.UseProgram(_handle);
                _isBeingUsed = true;
            }
        }

        public void Detach()
        {
            GraphicsContext.Gl.UseProgram(0);
            _isBeingUsed = false;
        }

        public void SetUniform(string name, int value)
        {
            int location = GraphicsContext.Gl.GetUniformLocation(_handle, name);
            if (location is -1)
            {
                throw new Exception($"{name} uniform not found on shader.");
            }

            Use();
            GraphicsContext.Gl.Uniform1(location, value);
        }

        public void SetUniform(string name, Span<int> value)
        {
            int location = GraphicsContext.Gl.GetUniformLocation(_handle, name);
            // if (location is -1)
            // {
            // throw new Exception($"{name} uniform not found on shader.");
            // }

            Use();
            GraphicsContext.Gl.Uniform1(location, (uint) value.Length, value);
        }

        public unsafe void SetUniform(string name, Matrix4x4 matrix)
        {
            int location = GraphicsContext.Gl.GetUniformLocation(_handle, name);
            if (location is -1)
            {
                throw new Exception($"{name} uniform not found on shader.");
            }

            Use();
            GraphicsContext.Gl.UniformMatrix4(location, 1, false, (float*) &matrix);
        }

        public void SetUniform(string name, Vector4 vector)
        {
            int location = GraphicsContext.Gl.GetUniformLocation(_handle, name);
            if (location is -1)
            {
                throw new Exception($"{name} uniform not found on shader.");
            }

            Use();
            GraphicsContext.Gl.Uniform4(location, vector);
        }

        public void SetUniform(string name, Vector3 vector)
        {
            int location = GraphicsContext.Gl.GetUniformLocation(_handle, name);
            if (location is -1)
            {
                throw new Exception($"{name} uniform not found on shader.");
            }

            Use();
            GraphicsContext.Gl.Uniform3(location, vector);
        }

        public void SetUniform(string name, Vector2 vector)
        {
            int location = GraphicsContext.Gl.GetUniformLocation(_handle, name);
            if (location is -1)
            {
                throw new Exception($"{name} uniform not found on shader.");
            }

            Use();
            GraphicsContext.Gl.Uniform2(location, vector);
        }

        public void SetUniform(string name, float value)
        {
            int location = GraphicsContext.Gl.GetUniformLocation(_handle, name);
            if (location is -1)
            {
                throw new Exception($"{name} uniform not found on shader.");
            }

            Use();
            GraphicsContext.Gl.Uniform1(location, value);
        }

        private void LinkProgram(uint program)
        {
            GraphicsContext.Gl.LinkProgram(program);

            GraphicsContext.Gl.GetProgram(program, GLEnum.LinkStatus, out int status);
            if (status is 0)
            {
                throw new Exception(
                    $"Program failed to link with error: {GraphicsContext.Gl.GetProgramInfoLog(program)}");
            }
        }

        private uint LoadShader(ShaderType type, string path)
        {
            string src = File.ReadAllText(path);

            return CompileShaderFromSource(type, src);
        }

        private uint CompileShaderFromSource(ShaderType type, string src)
        {
            uint handle = GraphicsContext.Gl.CreateShader(type);
            GraphicsContext.Gl.ShaderSource(handle, src);
            GraphicsContext.Gl.CompileShader(handle);

            string infoLog = GraphicsContext.Gl.GetShaderInfoLog(handle);
            if (!string.IsNullOrWhiteSpace(infoLog))
            {
                throw new Exception($"Error compiling shader of type {type}, failed with error {infoLog}");
            }

            return handle;
        }

        private static async Task<(string, string)> GetShaderParts(string fullShaderPath)
        {
            var vertexShaderSource = "";
            var fragmentShaderSource = "";

            try
            {
                string src = await File.ReadAllTextAsync(fullShaderPath);
                string[] splitString = Regex.Split(src, @"(#type)( )+([a-zA-Z]+)", RegexOptions.ExplicitCapture);

                int index = src.IndexOf("#type", StringComparison.Ordinal) + 6;
                int eol = src.IndexOf("\r\n", index, StringComparison.Ordinal);

                string firstPattern = src.Substring(index, eol - index).Trim();

                index = src.IndexOf("#type", eol, StringComparison.Ordinal) + 6;
                eol = src.IndexOf("\r\n", index, StringComparison.Ordinal);

                string secondPattern = src.Substring(index, eol - index).Trim();

                if (firstPattern.Equals("vertex"))
                {
                    vertexShaderSource = splitString[1];
                }
                else if (firstPattern.Equals("fragment"))
                {
                    fragmentShaderSource = splitString[1];
                }
                else
                {
                    throw new Exception("Incorrect shader");
                }

                if (secondPattern.Equals("vertex"))
                {
                    vertexShaderSource = splitString[2];
                }
                else if (secondPattern.Equals("fragment"))
                {
                    fragmentShaderSource = splitString[2];
                }
                else
                {
                    throw new Exception("Incorrect shader");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return (vertexShaderSource, fragmentShaderSource);
        }
    }
}