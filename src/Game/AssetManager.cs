using OpenTK.Graphics.OpenGL4;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Diagnostics.CodeAnalysis;

namespace Game
{
    internal class AssetManager
    {
        public readonly static string AssetFolder = "Assets";
        public readonly static string ShaderFolder = "Shaders";
        public readonly static string TextureFolder = "Textures";

        public static readonly Dictionary<string, Shader> Shaders = new Dictionary<string, Shader>();

        [return: NotNull]
        public static Shader CreateOrGetShader(string name, ShaderType shaderType, bool reload)
        {
            if (!reload && Shaders.TryGetValue(name, out Shader? shader))
            {
                return shader;
            }

            string path = GetPath(name, ShaderFolder);

            string vertexCode = File.ReadAllText($"{path}.vert");
            string fragCode = File.ReadAllText($"{path}.frag");

            shader = new Shader(shaderType, vertexCode, fragCode);

            if (!Shaders.TryAdd(name, shader))
            {
                Shaders.Add(name, shader);
            }

            return shader;
        }

        public static Image LoadImageJpg(string name)
        {
            string path = GetPath(name, TextureFolder);

            using Image<Rgb24> image = SixLabors.ImageSharp.Image.Load<Rgb24>($"{path}.jpg");

            var pixels = new byte[image.Width * image.Height * 3];
            image.CopyPixelDataTo(pixels);

            return new Image(pixels, image.Width, image.Height);
        }

        public static Texture2D CreateTextureFromJpg(string name, PixelInternalFormat internalFormat)
        {
            string path = GetPath(name, TextureFolder);

            using Image<Rgba32> image = SixLabors.ImageSharp.Image.Load<Rgba32>($"{path}.jpg");
            image.Mutate(x => x.Flip(FlipMode.Vertical));

            var pixels = new byte[image.Width * image.Height * 4];
            image.CopyPixelDataTo(pixels);

            return new Texture2D(internalFormat, image.Width, image.Height, pixels);
        }

        public static Texture2D CreateTextureFromPng(string name, PixelInternalFormat internalFormat)
        {
            string path = GetPath(name, TextureFolder);

            using Image<Rgba32> image = SixLabors.ImageSharp.Image.Load<Rgba32>($"{path}.png");
            image.Mutate(x => x.Flip(FlipMode.Vertical));

            var pixels = new byte[image.Width * image.Height * 4];
            image.CopyPixelDataTo(pixels);

            return new Texture2D(internalFormat, image.Width, image.Height, pixels);
        }

        public static string GetPath(string name, string folder) 
            => Path.Combine(Directory.GetCurrentDirectory(), Path.Combine(Path.Combine(AssetFolder, folder), name));
    }
}
