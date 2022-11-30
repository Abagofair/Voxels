namespace Game
{
    internal abstract class Material
    {
        protected Shader _shader;

        public Material(
            Shader shader)
        {
            _shader = shader ?? throw new ArgumentNullException(nameof(shader));
        }

        public Shader Shader => _shader;

        public virtual void Apply()
        {
            _shader.Use();
        }
    }
}
