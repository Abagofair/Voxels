#version 450

layout(location = 5) uniform mat4 Model;
layout(location = 6) uniform mat4 View; 
layout(location = 7) uniform mat4 Projection;

out vec3 nearPoint;
out vec3 farPoint;

vec2 quad[4] = vec2[4](
	vec2(1.0, 1.0),
	vec2(1.0, -1.0),
	vec2(-1.0, -1.0),
	vec2(-1.0, 1.0)
);

vec3 unprojectPoint(float x, float y, float z, mat4 v, mat4 p) {
    vec4 unprojectedPoint = inverse(v) * inverse(p) * vec4(x, y, z, 1.0);
    return unprojectedPoint.xyz / unprojectedPoint.w;
}

void main() {
   vec3 p = vec3(quad[gl_VertexID].xy, 0.0);

   nearPoint = unprojectPoint(p.x, p.y, 0.0, View, Projection).xyz; // unprojecting on the near plane
   farPoint = unprojectPoint(p.x, p.y, 1.0, View, Projection).xyz; // unprojecting on the far plane
   
   gl_Position = vec4(p, 1);
}