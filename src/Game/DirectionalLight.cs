using OpenTK.Mathematics;

namespace Game
{
    internal class DirectionalLight
    {
        public Vector3 direction = new Vector3(0.0f, -1.0f, 0.0f);
        public LightProperties lightProperties = new LightProperties();

        public void Upload(Shader shader)
        {
            shader.SetVec3("directionalLight.direction", ref direction);
            shader.SetVec3("directionalLight.ambient", ref lightProperties.ambient);
            shader.SetVec3("directionalLight.diffuse", ref lightProperties.diffuse);
            shader.SetVec3("directionalLight.specular", ref lightProperties.specular);
        }
    }
}
