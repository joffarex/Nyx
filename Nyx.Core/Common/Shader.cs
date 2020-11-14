using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Nyx.Core.Logger;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Nyx.Core.Common
{
    public class Shader : IDisposable, IEquatable<Shader>
    {
        private static readonly ILogger<Shader> Logger = SerilogLogger.Factory.CreateLogger<Shader>();

        private bool _isBeingUsed;
        public bool IsDisposed { get; private set; }

        public int Handle { get; private set; }

        private readonly Dictionary<string, int> _uniformLocations = new();

        public Shader(string shaderPath)
        {
            (string vertexSource, string fragmentSource) = GetShaderParts(shaderPath).GetAwaiter().GetResult();

            int vertexShader = CreateShader(ShaderType.VertexShader, vertexSource);
            int fragmentShader = CreateShader(ShaderType.FragmentShader, fragmentSource);

            Init(vertexShader, fragmentShader);
        }

        public Shader(string vertPath, string fragPath)
        {
            string shaderSource = LoadSource(vertPath);
            int vertexShader = CreateShader(ShaderType.VertexShader, shaderSource);

            shaderSource = LoadSource(fragPath);
            int fragmentShader = CreateShader(ShaderType.FragmentShader, shaderSource);

            Init(vertexShader, fragmentShader);
        }

        ~Shader()
        {
            Dispose(false);
            Logger.LogError($"Memory leak detected on object: {this}");
        }

        private void Init(int vertexShader, int fragmentShader)
        {
            Handle = GL.CreateProgram();

            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragmentShader);

            LinkProgram(Handle);

            GL.DetachShader(Handle, vertexShader);
            GL.DetachShader(Handle, fragmentShader);
            GL.DeleteShader(fragmentShader);
            GL.DeleteShader(vertexShader);

            CacheUniforms();
        }

        private void CacheUniforms()
        {
            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out int numberOfUniforms);
            for (var i = 0; i < numberOfUniforms; i++)
            {
                string key = GL.GetActiveUniform(Handle, i, out _, out _);
                int location = GL.GetUniformLocation(Handle, key);

                _uniformLocations.Add(key, location);
            }
        }

        private static void CompileShader(int shader)
        {
            GL.CompileShader(shader);

            GL.GetShader(shader, ShaderParameter.CompileStatus, out var code);

            if (code != (int) All.True)
            {
                string infoLog = GL.GetShaderInfoLog(shader);
                throw new Exception($"Error occurred whilst compiling Shader({shader}).\n\n{infoLog}");
            }
        }

        private static void LinkProgram(int program)
        {
            GL.LinkProgram(program);

            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var code);
            if (code != (int) All.True)
            {
                throw new Exception($"Error occurred whilst linking Program({program})");
            }
        }

        private static int CreateShader(ShaderType shaderType, string shaderSource)
        {
            int vertexShader = GL.CreateShader(shaderType);
            GL.ShaderSource(vertexShader, shaderSource);

            CompileShader(vertexShader);

            return vertexShader;
        }

        private static string LoadSource(string path)
        {
            using var sr = new StreamReader(path, Encoding.UTF8);
            return sr.ReadToEnd();
        }

        public void Use()
        {
            if (!_isBeingUsed)
            {
                GL.UseProgram(Handle);
                _isBeingUsed = true;
            }
        }

        public void Detach()
        {
            GL.UseProgram(0);
            _isBeingUsed = false;
        }

        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(Handle, attribName);
        }

        public void SetInt(string name, int data)
        {
            if (_uniformLocations.TryGetValue(name, out int location))
            {
                Use();
                GL.Uniform1(_uniformLocations[name], data);
            }
            else
            {
                throw new Exception($"{name} uniform not found on shader.");
            }
        }

        public void SetFloat(string name, float data)
        {
            if (_uniformLocations.TryGetValue(name, out int location))
            {
                Use();
                GL.Uniform1(_uniformLocations[name], data);
            }
            else
            {
                throw new Exception($"{name} uniform not found on shader.");
            }
        }

        public void SetMatrix4(string name, Matrix4 data)
        {
            if (_uniformLocations.TryGetValue(name, out int location))
            {
                Use();
                GL.UniformMatrix4(_uniformLocations[name], true, ref data);
            }
            else
            {
                throw new Exception($"{name} uniform not found on shader.");
            }
        }

        public void SetVector3(string name, Vector3 data)
        {
            if (_uniformLocations.TryGetValue(name, out int location))
            {
                Use();
                GL.Uniform3(location, data);
            }
            else
            {
                throw new Exception($"{name} uniform not found on shader.");
            }
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

                switch (firstPattern)
                {
                    case "vertex":
                        vertexShaderSource = splitString[1];
                        break;
                    case "fragment":
                        fragmentShaderSource = splitString[1];
                        break;
                    default:
                        throw new Exception("Incorrect shader");
                }

                switch (secondPattern)
                {
                    case "vertex":
                        vertexShaderSource = splitString[2];
                        break;
                    case "fragment":
                        fragmentShaderSource = splitString[2];
                        break;
                    default:
                        throw new Exception("Incorrect shader");
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
            }

            return (vertexShaderSource, fragmentShaderSource);
        }

        public void Dispose()
        {
            if (IsDisposed) return;

            IsDisposed = true;
            Dispose(true);
            // prevent the destructor from being called
            GC.SuppressFinalize(this);
            // make sure the garbage collector does not eat our object before it is properly disposed
            GC.KeepAlive(this);
        }

        public void Dispose(bool manual)
        {
            if (!manual) return;
            GL.DeleteShader(Handle);
        }
        
        public bool Equals(Shader other)
        {
            return other != null && Handle.Equals(other.Handle);
        }

        public override bool Equals(object obj)
        {
            return obj is Shader shader && Equals(shader);
        }

        public override int GetHashCode()
        {
            return Handle.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format($"[{GetType().Name}]: {Handle}");
        }
    }
}