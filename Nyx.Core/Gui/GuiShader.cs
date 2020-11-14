using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using OpenTK.Graphics.OpenGL4;

namespace Nyx.Core.Gui
{
    internal class GuiShader
    {
        private readonly string _name;
        private readonly Dictionary<string, int> _uniformToLocation = new();
        private bool _initialized;

        public GuiShader(string name, string vertexShader, string fragmentShader)
        {
            _name = name;
            (ShaderType Type, string Path)[] files =
            {
                (ShaderType.VertexShader, vertexShader),
                (ShaderType.FragmentShader, fragmentShader),
            };
            Program = CreateProgram(name, files);
        }

        private int Program { get; }

        public void UseShader()
        {
            GL.UseProgram(Program);
        }

        public void Dispose()
        {
            if (_initialized)
            {
                GL.DeleteProgram(Program);
                _initialized = false;
            }
        }

        public UniformFieldInfo[] GetUniforms()
        {
            GL.GetProgram(Program, GetProgramParameterName.ActiveUniforms, out int uniformCount);

            var uniforms = new UniformFieldInfo[uniformCount];

            for (var i = 0; i < uniformCount; i++)
            {
                string name = GL.GetActiveUniform(Program, i, out int size, out ActiveUniformType type);

                UniformFieldInfo fieldInfo;
                fieldInfo.Location = GetUniformLocation(name);
                fieldInfo.Name = name;
                fieldInfo.Size = size;
                fieldInfo.Type = type;

                uniforms[i] = fieldInfo;
            }

            return uniforms;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetUniformLocation(string uniform)
        {
            if (_uniformToLocation.TryGetValue(uniform, out int location) == false)
            {
                location = GL.GetUniformLocation(Program, uniform);
                _uniformToLocation.Add(uniform, location);

                if (location == -1)
                {
                    Debug.Print($"The uniform '{uniform}' does not exist in the shader '{_name}'!");
                }
            }

            return location;
        }

        private int CreateProgram(string name, params (ShaderType Type, string source)[] shaderPaths)
        {
            Utils.CreateProgram(name, out int program);

            var shaders = new int[shaderPaths.Length];
            for (var i = 0; i < shaderPaths.Length; i++)
            {
                shaders[i] = CompileShader(name, shaderPaths[i].Type, shaderPaths[i].source);
            }

            foreach (int shader in shaders)
            {
                GL.AttachShader(program, shader);
            }

            GL.LinkProgram(program);

            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int success);
            if (success == 0)
            {
                string info = GL.GetProgramInfoLog(program);
                Debug.WriteLine($"GL.LinkProgram had info log [{name}]:\n{info}");
            }

            foreach (int shader in shaders)
            {
                GL.DetachShader(program, shader);
                GL.DeleteShader(shader);
            }

            _initialized = true;

            return program;
        }

        private int CompileShader(string name, ShaderType type, string source)
        {
            Utils.CreateShader(type, name, out int shader);
            GL.ShaderSource(shader, source);
            GL.CompileShader(shader);

            GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string info = GL.GetShaderInfoLog(shader);
                Debug.WriteLine($"GL.CompileShader for shader '{_name}' [{type}] had info log:\n{info}");
            }

            return shader;
        }
    }
}