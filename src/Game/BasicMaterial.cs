using OpenTK.Graphics.OpenGL4;

namespace Game
{
    internal class BasicMaterial : Material
    {
        private MaterialProperties _shaderMaterial;

        public BasicMaterial(
            Shader shader, 
            MaterialProperties shaderMaterial) 
            : base(shader)
        {
            _shaderMaterial = shaderMaterial ?? throw new ArgumentNullException(nameof(shaderMaterial));
        }

        public override void Apply()
        {
            base.Apply();
            UploadMaterialPropertiers();
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
                _shader.SetInt("material.specular", 1);
                GL.ActiveTexture(TextureUnit.Texture1);
                _shaderMaterial.specular.Bind();
            }
        }
    }
}
