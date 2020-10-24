using System;
using System.IO;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Silk.NET.OpenGL;

namespace Nyx.Core.OpenGL
{
    public class Shader : IDisposable
    {
        private readonly GL _gl;
        private uint _handle;
        private bool _isBeingUsed;

        // TODO: add uniform caching
        // This is necessary as accessing shader for uniform locations is not performant

        public Shader(GL gl, string shaderPath)
        {
            _gl = gl;

            (string vertexSource, string fragmentSource) = GetShaderParts(shaderPath).GetAwaiter().GetResult();
            uint vertex = CompileShaderFromSource(ShaderType.VertexShader, vertexSource);
            uint fragment = CompileShaderFromSource(ShaderType.FragmentShader, fragmentSource);

            Init(vertex, fragment);
        }

        public Shader(GL gl, string vertexPath, string fragmentPath)
        {
            _gl = gl;

            uint vertex = LoadShader(ShaderType.VertexShader, vertexPath);
            uint fragment = LoadShader(ShaderType.FragmentShader, fragmentPath);

            Init(vertex, fragment);
        }

        public void Dispose()
        {
            _gl.DeleteProgram(_handle);
        }


        private void Init(uint vertex, uint fragment)
        {
            // Combine shaders under one shader program
            _handle = _gl.CreateProgram();
            _gl.AttachShader(_handle, vertex);
            _gl.AttachShader(_handle, fragment);
            LinkProgram(_handle);

            // Delete the no longer useful individual shaders;
            _gl.DetachShader(_handle, vertex);
            _gl.DetachShader(_handle, fragment);
            _gl.DeleteShader(vertex);
            _gl.DeleteShader(fragment);
        }

        public void Use()
        {
            if (!_isBeingUsed)
            {
                _gl.UseProgram(_handle);
                _isBeingUsed = true;
            }
        }

        public void Detach()
        {
            _gl.UseProgram(0);
            _isBeingUsed = false;
        }

        public void SetUniform(string name, int value)
        {
            int location = _gl.GetUniformLocation(_handle, name);
            if (location == -1)
            {
                throw new Exception($"{name} uniform not found on shader.");
            }

            Use();
            _gl.Uniform1(location, value);
        }

        public unsafe void SetUniform(string name, Matrix4x4 matrix)
        {
            int location = _gl.GetUniformLocation(_handle, name);
            if (location == -1)
            {
                throw new Exception($"{name} uniform not found on shader.");
            }

            Use();
            _gl.UniformMatrix4(location, 1, false, (float*) &matrix);
        }

        public void SetUniform(string name, Vector4 vector)
        {
            int location = _gl.GetUniformLocation(_handle, name);
            if (location == -1)
            {
                throw new Exception($"{name} uniform not found on shader.");
            }

            Use();
            _gl.Uniform4(location, vector);
        }

        public void SetUniform(string name, Vector3 vector)
        {
            int location = _gl.GetUniformLocation(_handle, name);
            if (location == -1)
            {
                throw new Exception($"{name} uniform not found on shader.");
            }

            Use();
            _gl.Uniform3(location, vector);
        }

        public void SetUniform(string name, Vector2 vector)
        {
            int location = _gl.GetUniformLocation(_handle, name);
            if (location == -1)
            {
                throw new Exception($"{name} uniform not found on shader.");
            }

            Use();
            _gl.Uniform2(location, vector);
        }

        public void SetUniform(string name, float value)
        {
            int location = _gl.GetUniformLocation(_handle, name);
            if (location == -1)
            {
                throw new Exception($"{name} uniform not found on shader.");
            }

            Use();
            _gl.Uniform1(location, value);
        }

        private void LinkProgram(uint program)
        {
            _gl.LinkProgram(program);

            _gl.GetProgram(program, GLEnum.LinkStatus, out int status);
            if (status == 0)
            {
                throw new Exception($"Program failed to link with error: {_gl.GetProgramInfoLog(program)}");
            }
        }

        private uint LoadShader(ShaderType type, string path)
        {
            string src = File.ReadAllText(path);

            return CompileShaderFromSource(type, src);
        }

        private uint CompileShaderFromSource(ShaderType type, string src)
        {
            uint handle = _gl.CreateShader(type);
            _gl.ShaderSource(handle, src);
            _gl.CompileShader(handle);

            string infoLog = _gl.GetShaderInfoLog(handle);
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