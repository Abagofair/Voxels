namespace Game
{
    internal abstract class Light
    {
        public LightProperties lightProperties = new LightProperties();

        public virtual void Upload(Shader shader)
        {
            shader.SetVec3("light.ambient", ref lightProperties.ambient);
            shader.SetVec3("light.diffuse", ref lightProperties.diffuse);
            shader.SetVec3("light.specular", ref lightProperties.specular);
        }
    }
}
