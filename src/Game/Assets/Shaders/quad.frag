﻿#version 450 core

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

struct PointLight
{
    vec3 position;
    
    float constant;
    float linear;
    float quadratic;  

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

struct SpotLight
{
    vec3 position;
    vec3 direction;

    float cutOff;
    float outerCutOff;
  
    float constant;
    float linear;
    float quadratic;
  
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;       
};

layout(location = 8) uniform vec3 CameraPosition;

layout(location = 10) uniform DirectionalLight directionalLight;
layout(location = 15) uniform PointLight pointLight;
layout(location = 22) uniform SpotLight spotLight;

layout(location = 40) uniform Material material;

uniform samplerCube cubeMap;

in vec2 outUv;
in vec3 outNormal;
in vec3 outFragPos;

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
    vec3 ambient  = light.ambient  * vec3(texture(material.diffuse, outUv));
    vec3 diffuse  = light.diffuse  * diff * vec3(texture(material.diffuse, outUv));
    vec3 specular = light.specular * spec * vec3(texture(material.specular, outUv));
    
    return (ambient + diffuse + specular);
}

vec3 calculatePointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDirection)
{
    vec3 lightDir = normalize(light.position - fragPos);

    // diffuse shading
    float diff = max(dot(normal, lightDir), 0.0);

    // specular shading
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDirection, reflectDir), 0.0), material.shininess);

    // attenuation
    float distance    = length(light.position - fragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance + 
  			     light.quadratic * (distance * distance));   
                 
    // combine results
    vec3 ambient  = light.ambient  * vec3(texture(material.diffuse, outUv));
    vec3 diffuse  = light.diffuse  * diff * vec3(texture(material.diffuse, outUv));
    vec3 specular = light.specular * spec * vec3(texture(material.specular, outUv));

    ambient  *= attenuation;
    diffuse  *= attenuation;
    specular *= attenuation;

    return (ambient + diffuse + specular);
}

// calculates the color when using a spot light.
vec3 calculateSpotLight(SpotLight light, vec3 normal, vec3 fragPos, vec3 viewDir)
{
    vec3 lightDir = normalize(light.position - fragPos);

    // diffuse shading
    float diff = max(dot(normal, lightDir), 0.0);
    // specular shading

    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    
    // attenuation
    float distance = length(light.position - fragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));    
    
    // spotlight intensity
    float theta = dot(lightDir, normalize(-light.direction)); 
    float epsilon = light.cutOff - light.outerCutOff;
    float intensity = clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0);
    
    // combine results
    vec3 ambient = light.ambient * vec3(texture(material.diffuse, outUv));
    vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse, outUv));
    vec3 specular = light.specular * spec * vec3(texture(material.specular, outUv));
    
    ambient *= attenuation * intensity;
    diffuse *= attenuation * intensity;
    specular *= attenuation * intensity;
    
    return (ambient + diffuse + specular);
}

void main()
{
    vec3 norm = normalize(outNormal);
    vec3 viewDirection = normalize(CameraPosition - outFragPos);

    vec3 result = calculateDirectionalLight(directionalLight, norm, viewDirection);
    //result += calculatePointLight(pointLight, norm, outFragPos, viewDirection);
    //result += calculateSpotLight(spotLight, norm, outFragPos, viewDirection);

    FragColor = vec4(result, 1.0);
    float gamma = 2.2;
    FragColor.rgb = pow(FragColor.rgb, vec3(1.0 / gamma));
}