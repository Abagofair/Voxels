namespace Game
{
    internal class SkyboxMaterial : Material
    {
        private readonly CubeMap _cubeMap;

        public SkyboxMaterial(
            Shader shader,
            CubeMap cubeMap)
            : base(shader)
        {
            _cubeMap = cubeMap ?? throw new ArgumentNullException(nameof(cubeMap));
        }

        public override void Apply()
        {
            base.Apply();
            _cubeMap.Bind();
        }
    }
}
