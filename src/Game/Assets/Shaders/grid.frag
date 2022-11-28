#version 450

layout(location = 5) uniform mat4 Model;
layout(location = 6) uniform mat4 View; 
layout(location = 7) uniform mat4 Projection;

out vec4 outColor;
in vec3 nearPoint;
in vec3 farPoint;

vec4 grid(vec3 fragPos3D, float scale) {
    vec2 coord = fragPos3D.xz * scale; // use the scale variable to set the distance between the lines
    vec2 derivative = fwidth(coord);
    
    // for any whole number world space coordinate we ensure maximum opacity (1.0)
    // any other coordinate with a fraction will give us a more opaque line the closer we get to
    // the whole number and how much the neighbouring pixels change in value
    vec2 grid = abs(fract(coord - 0.5) - 0.5) / derivative;

    float line = min(grid.x, grid.y);
    
    vec4 color = vec4(0.5, 0.5, 0.5, 1.0 - min(line, 1.0));

    float minimumZ = min(derivative.y, 1);
    float minimumX = min(derivative.x, 1);
      
    // z axis
    if(fragPos3D.x > -0.5 * minimumX && fragPos3D.x < 0.5 * minimumX)
        color.z = 1.0;
    // x axis
    if(fragPos3D.z > -0.5 * minimumZ && fragPos3D.z < 0.5 * minimumZ)
        color.x = 1.0;
        
    return color;
}

float computeNdcDepth(vec3 pos) {
    vec4 clip_space_pos = Projection * View * Model * vec4(pos.xyz, 1.0);
    return (clip_space_pos.z / clip_space_pos.w);
}

float computeLinearDepth(vec3 pos) {
    float near = 0.01;
    float far = 1000.0;
    vec4 clip_space_pos = Projection * View * vec4(pos.xyz, 1.0);
    float clip_space_depth = (clip_space_pos.z / clip_space_pos.w) * 2.0 - 1.0; // put back between -1 and 1
    float linearDepth = (2.0 * near * far) / (far + near - clip_space_depth * (far - near)); // get linear value between 0.01 and 100
    return linearDepth / far; // normalize
}

void main() {
    float t = -nearPoint.y / (farPoint.y - nearPoint.y);
    vec3 fragPos3d = nearPoint + t * (farPoint - nearPoint);

    float far = gl_DepthRange.far; 
    float near = gl_DepthRange.near;

    float ndc_depth = computeNdcDepth(fragPos3d);

    //float depth = (((far-near) * ndc_depth) + near + far) / 2.0;
    float depth = 0.5*ndc_depth + 0.5;
    gl_FragDepth = depth;

    outColor = grid(fragPos3d, 1.0) * float(t > 0);
}