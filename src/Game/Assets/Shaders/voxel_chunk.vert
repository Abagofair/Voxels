#version 450 core

layout(location = 0) in vec3 position;
layout(location = 1) in vec3 normal;
layout(location = 2) in vec2 uv;
layout(location = 3) in vec4 color;

layout(location = 5) uniform mat4 Model;
layout(location = 6) uniform mat4 View; 
layout(location = 7) uniform mat4 Projection;
layout(location = 8) uniform vec3 CameraPosition;

out vec2 outUv;
out vec3 outNormal;
out vec3 outFragPos;
out vec4 outColor;

void main()
{
    outFragPos = vec3(Model * vec4(position, 1.0f));
    outNormal = mat3(transpose(inverse(Model))) * normal;
    outUv = uv;
    outColor = color;
    gl_Position = Projection * View * Model * vec4(position, 1.0);
}