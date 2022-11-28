#version 450 core

out vec4 FragColor;

in vec3 outUvw;

uniform samplerCube skybox;

void main()
{   
    float gamma = 2.2;
    FragColor = texture(skybox, outUvw);
    FragColor.rgb = pow(FragColor.rgb, vec3(1.0 / gamma));
}