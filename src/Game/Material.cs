using OpenTK.Graphics.OpenGL4;

namespace Game
{
    internal class Material
    {
        private Shader _shader;
        private MaterialProperties _shaderMaterial;

        public Material(
            Shader shader,
            MaterialProperties shaderMaterial)
        {
            _shader = shader ?? throw new ArgumentNullException(nameof(shader));
            _shaderMaterial = shaderMaterial;
        }

        public Shader Shader => _shader;

        public void Apply()
        {
            UploadMaterialPropertiers();
            _shader.Use();
        }

        private void UploadMaterialPropertiers()
        {
            _shader.SetVec3("material.ambient", ref _shaderMaterial.ambient);
            _shader.SetFloat("material.shininess", _shaderMaterial.shininess);

            if (_shaderMaterial.diffuse != null)
            {
                _shader.SetInt("material.diffuse", 0);
                GL.ActiveTexture(TextureUnit.Texture0);
                _shaderMaterial.diffuse.Bind();
            }

            if (_shaderMaterial.specular != null)
            {
                _shader.SetInt("material.specular", 0);
                GL.ActiveTexture(TextureUnit.Texture1);
                _shaderMaterial.specular.Bind();
            }
        }
    }
}
