using OpenTK.Mathematics;

namespace Game
{
    internal struct LightProperties
    {
        public Vector3 ambient;
        public Vector3 diffuse;
        public Vector3 specular;

        public LightProperties() 
        {
            ambient = Vector3.One;
            diffuse = Vector3.One;
            specular = Vector3.One;
        }
    }
}
