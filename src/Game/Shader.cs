using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Game
{
    internal class Shader : IDisposable
    {
        private readonly int _handle;
        private readonly ShaderType _shaderType;

        public Shader(
            ShaderType shaderType,
            string vertSource, 
            string fragSource)
        {
            if (vertSource == null) throw new ArgumentNullException(nameof(vertSource));
            if (fragSource == null) throw new ArgumentNullException(nameof(fragSource));

            var vertexShader = Compile(vertSource, OpenTK.Graphics.OpenGL4.ShaderType.VertexShader);
            var fragmentShader = Compile(fragSource, OpenTK.Graphics.OpenGL4.ShaderType.FragmentShader);

            _handle = Link(vertexShader, fragmentShader);

            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

            _shaderType = shaderType;
        }

        public ShaderType Type => _shaderType;

        public void Use()
        {
            GL.UseProgram(_handle);
        }

        public void SetFloat(int location, float value) => GL.ProgramUniform1(_handle, location, value);
        public void SetFloat(string name, float value)
        {
            int loc = GL.GetUniformLocation(_handle, name);
            GL.ProgramUniform1(_handle, loc, value);
        }

        public void SetInt(int location, int value) => GL.ProgramUniform1(_handle, location, value);
        public void SetInt(string name, int value)
        {
            int loc = GL.GetUniformLocation(_handle, name);
            GL.ProgramUniform1(_handle, loc, value);
        }


        public void SetVec3(int location, ref Vector3 value) => GL.ProgramUniform3(_handle, location, value);
        public void SetVec3(string name, ref Vector3 value)
        {
            int loc = GL.GetUniformLocation(_handle, name);
            GL.ProgramUniform3(_handle, loc, value);
        }

        public void SetVec4(int location, Color4 value) => GL.ProgramUniform4(_handle, location, value);
        public void SetVec4(int location, Vector4 value) => GL.ProgramUniform4(_handle, location, value);
        public void SetVec4(string name, ref Vector4 value)
        {
            int loc = GL.GetUniformLocation(_handle, name);
            GL.ProgramUniform4(_handle, loc, value);
        }

        public void SetMatrix4f(int location, ref Matrix4 value) => GL.ProgramUniformMatrix4(_handle, location, false, ref value);
        public void SetMatrix4f(string name, ref Matrix4 value)
        {
            int loc = GL.GetUniformLocation(_handle, name);
            GL.ProgramUniformMatrix4(_handle, loc, false, ref value);
        }

        public static int Compile(string source, OpenTK.Graphics.OpenGL4.ShaderType shaderType)
        {
            var shader = GL.CreateShader(shaderType);
            GL.ShaderSource(shader, source);
            GL.CompileShader(shader);

            GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);
            if (success == 0) 
            {
                Logger.Debug(GL.GetShaderInfoLog(shader));
                throw new ArgumentException(nameof(source));
            }

            return shader;
        }

        public static int Link(int vertHandle, int fragHandle) 
        {
            var handle = GL.CreateProgram();

            GL.AttachShader(handle, vertHandle);
            GL.AttachShader(handle, fragHandle);

            GL.LinkProgram(handle);

            GL.GetProgram(handle, GetProgramParameterName.LinkStatus, out int success);
            if (success == 0)
            {
                Logger.Debug(GL.GetShaderInfoLog(handle));
                throw new ArgumentException();
            }

            GL.DetachShader(handle, vertHandle);
            GL.DetachShader(handle, fragHandle);

            return handle;
        }

        public void Dispose()
        {
            GL.DeleteProgram(_handle);
        }
    }
}
