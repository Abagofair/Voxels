using OpenTK.Mathematics;

namespace Game
{
    internal class PointLight
    {
        public Vector3 position = new Vector3(0.0f, 0.0f, 0.0f);
        public LightProperties lightProperties = new LightProperties();
        public const float constant = 1.0f;
        public float linear = 0.09f;
        public float quadratic = 0.032f;

        public void Upload(Shader shader)
        {
            shader.SetVec3("pointLight.position", ref position);
            shader.SetVec3("pointLight.ambient", ref lightProperties.ambient);
            shader.SetVec3("pointLight.diffuse", ref lightProperties.diffuse);
            shader.SetVec3("pointLight.specular", ref lightProperties.specular);
            shader.SetFloat("pointLight.constant", constant);
            shader.SetFloat("pointLight.linear", linear);
            shader.SetFloat("pointLight.quadratic", quadratic);
        }
    }
}
