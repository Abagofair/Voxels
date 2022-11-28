using OpenTK.Mathematics;

namespace Game
{
    internal class MaterialProperties
    {
        public Vector3 ambient;
        public Texture2D? diffuse;
        public Texture2D? specular;
        public float shininess;

        public MaterialProperties()
        {
            ambient = Vector3.One;
            shininess = 0.0f;
        }

        public MaterialProperties(
            Texture2D diffuse,
            Texture2D specular,
            float shininess)
        {
            this.diffuse = diffuse;
            this.specular = specular;
            this.shininess = shininess;
            ambient = Vector3.One;
        }
    }
}
