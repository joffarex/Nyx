using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Nyx.Core.Logger;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Vector3 = OpenTK.Mathematics.Vector3;

namespace Nyx.Core.Renderer
{
    public class Shader : IDisposable, IEquatable<Shader>
    {
        private static readonly ILogger<Shader> Logger = SerilogLogger.Factory.CreateLogger<Shader>();

        private readonly List<UniformFieldInfo> _uniforms = new();

        private bool _isBeingUsed;

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

        public Shader(string type, string vertSource, string fragSource)
        {
            Logger.LogInformation($"Creating {type} shader");
            int vertexShader = CreateShader(ShaderType.VertexShader, vertSource);
            int fragmentShader = CreateShader(ShaderType.FragmentShader, fragSource);

            Init(vertexShader, fragmentShader);
        }

        public bool IsDisposed { get; private set; }

        public int Handle { get; private set; }

        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            IsDisposed = true;
            Dispose(true);
            // prevent the destructor from being called
            GC.SuppressFinalize(this);
            // make sure the garbage collector does not eat our object before it is properly disposed
            GC.KeepAlive(this);
        }

        public bool Equals(Shader other)
        {
            return (other != null) && Handle.Equals(other.Handle);
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
                string name = GL.GetActiveUniform(Handle, i, out int size, out ActiveUniformType type);
                int location = GL.GetUniformLocation(Handle, name);

                _uniforms.Add(new UniformFieldInfo {Name = name, Size = size, Type = type, Location = location});
            }
        }

        private static void CompileShader(int shader)
        {
            GL.CompileShader(shader);

            GL.GetShader(shader, ShaderParameter.CompileStatus, out int code);

            if (code != (int) All.True)
            {
                string infoLog = GL.GetShaderInfoLog(shader);
                throw new Exception($"Error occurred whilst compiling Shader({shader}).\n\n{infoLog}");
            }
        }

        private static void LinkProgram(int program)
        {
            GL.LinkProgram(program);

            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int code);
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
            try
            {
                UniformFieldInfo? uniform = _uniforms.SingleOrDefault(u => u.Name.Equals(name));
                Use();
                GL.Uniform1(uniform.Value.Location, data);
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
                Logger.LogDebug($"{name} uniform not found on shader.");
            }
        }

        public void SetFloat(string name, float data)
        {
            try
            {
                UniformFieldInfo? uniform = _uniforms.SingleOrDefault(u => u.Name.Equals(name));
                Use();
                GL.Uniform1(uniform.Value.Location, data);
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
                Logger.LogDebug($"{name} uniform not found on shader.");
            }
        }

        public void SetMatrix4(string name, Matrix4 data)
        {
            try
            {
                UniformFieldInfo? uniform = _uniforms.SingleOrDefault(u => u.Name.Equals(name));
                Use();
                GL.UniformMatrix4(uniform.Value.Location, true, ref data);
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
                Logger.LogDebug($"{name} uniform not found on shader.");
            }
        }

        public unsafe void SetMatrix4(string name, int count, bool transpose, ref Matrix4x4 data)
        {
            try
            {
                UniformFieldInfo? uniform = _uniforms.SingleOrDefault(u => u.Name.Equals(name));
                Use();
                GL.UniformMatrix4(uniform.Value.Location, count, transpose, (float*) Unsafe.AsPointer(ref data));
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
                Logger.LogDebug($"{name} uniform not found on shader.");
            }
        }

        public void SetVector3(string name, Vector3 data)
        {
            try
            {
                UniformFieldInfo? uniform = _uniforms.SingleOrDefault(u => u.Name.Equals(name));
                Use();
                GL.Uniform3(uniform.Value.Location, data);
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
                Logger.LogDebug($"{name} uniform not found on shader.");
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

        public void Dispose(bool manual)
        {
            if (!manual)
            {
                return;
            }

            GL.DeleteShader(Handle);
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