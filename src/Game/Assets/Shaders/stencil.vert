#version 450
layout(location = 0) in vec3 position;

layout(location = 5) uniform mat4 Model;
layout(location = 6) uniform mat4 View; 
layout(location = 7) uniform mat4 Projection;

void main()
{
	gl_Position = Projection * View * Model * vec4(position, 1.0);
}