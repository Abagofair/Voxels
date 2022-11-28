using OpenTK.Mathematics;
using System.Reflection.Metadata;

namespace Game
{
    internal class SpotLight
    {
        public Vector3 direction = Vector3.Zero;
        public Vector3 position = new Vector3(0.0f, 0.0f, 0.0f);

        public LightProperties lightProperties = new LightProperties();

        public const float constant = 1.0f;
        public float linear = 0.09f;
        public float quadratic = 0.032f;

        public float cutOff = 0.995f;
        public float outerCutOff = 0.940f;

        public void Upload(Shader shader)
        {
            shader.SetVec3("spotLight.direction", ref direction);
            shader.SetFloat("spotLight.cutOff", cutOff);
            shader.SetFloat("spotLight.outerCutOff", outerCutOff);

            shader.SetVec3("spotLight.position", ref position);
            shader.SetVec3("spotLight.ambient", ref lightProperties.ambient);
            shader.SetVec3("spotLight.diffuse", ref lightProperties.diffuse);
            shader.SetVec3("spotLight.specular", ref lightProperties.specular);
            shader.SetFloat("spotLight.constant", constant);
            shader.SetFloat("spotLight.linear", linear);
            shader.SetFloat("spotLight.quadratic", quadratic);
        }
    }
}
