#version 450 core
layout (location = 0) in vec3 position;
layout (location = 1) in vec3 normal;
layout (location = 2) in vec2 uv;

layout (location = 6) uniform mat4 View;
layout (location = 7) uniform mat4 Projection;


out vec3 outUvw;

void main()
{
    outUvw = position;
    vec4 pos = Projection * View * vec4(position, 1.0);
    gl_Position = pos.xyww;
}