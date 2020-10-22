using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Silk.NET.OpenGL;

namespace Nyx.Core.OpenGL
{
    public class Shader : IDisposable
    {
        private readonly GL _gl;
        private uint _handle;

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
            _gl.UseProgram(_handle);
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

        public unsafe void SetUniform(string name, Matrix4x4 value)
        {
            //A new overload has been created for setting a uniform so we can use the transform in our shader.
            int location = _gl.GetUniformLocation(_handle, name);
            if (location == -1)
            {
                throw new Exception($"{name} uniform not found on shader.");
            }

            Use();
            _gl.UniformMatrix4(location, 1, false, (float*) &value);
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
            List<string> lines = (await File.ReadAllLinesAsync(fullShaderPath)).ToList();

            var vertexShaderIndicator = "#type vertex";

            if (vertexShaderIndicator != lines[0])
            {
                throw new Exception("GLSL formatting is incorrect. file must start with vertx shader");
            }

            var fragmentShaderIndicator = "#type fragment";
            int fragmentShaderIndicatorIndex = lines.IndexOf(fragmentShaderIndicator);

            var vertexShaderSource = "";

            for (var i = 1; i < fragmentShaderIndicatorIndex; i++)
            {
                vertexShaderSource += $"{lines[i]}\n";
            }

            var fragmentShaderSource = "";

            for (int i = fragmentShaderIndicatorIndex + 1; i < lines.Count; i++)
            {
                fragmentShaderSource += $"{lines[i]}\n";
            }

            return (vertexShaderSource, fragmentShaderSource);
        }
    }
}