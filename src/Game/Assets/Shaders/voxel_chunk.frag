#version 450 core

struct Material
{
    sampler2D diffuse;
    sampler2D specular;
    float shininess;
    vec3 ambient;
};

struct DirectionalLight
{
    vec3 direction;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

layout(location = 8) uniform vec3 CameraPosition;

layout(location = 10) uniform DirectionalLight directionalLight;

layout(location = 40) uniform Material material;

uniform samplerCube cubeMap;

in vec2 outUv;
in vec3 outNormal;
in vec3 outFragPos;
in vec4 outColor;

out vec4 FragColor;

vec3 calculateDirectionalLight(DirectionalLight light, vec3 normal, vec3 viewDirection)
{
    vec3 lightDir = normalize(-light.direction);
    
    // diffuse shading
    float diff = max(dot(normal, lightDir), 0.0);

    // specular shading
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDirection, reflectDir), 0.0), material.shininess);
    
    // combine results
    vec3 ambient  = light.ambient  * outColor.rgb;
    vec3 diffuse  = light.diffuse  * diff * outColor.rgb;
    vec3 specular = light.specular * spec * outColor.rgb;
    
    return (ambient + diffuse + specular);
}

void main()
{
    vec3 norm = normalize(outNormal);
    vec3 viewDirection = normalize(CameraPosition - outFragPos);

    vec3 result = calculateDirectionalLight(directionalLight, norm, viewDirection);

    FragColor = vec4(result, 1.0);
    float gamma = 2.2;
    FragColor.rgb = pow(FragColor.rgb, vec3(1.0 / gamma));
}