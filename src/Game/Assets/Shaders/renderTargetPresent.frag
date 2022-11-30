#version 450

in vec2 outUv;

uniform sampler2D finalColorTexture;

out vec4 FragColor;

void main()
{
	//gamma correct here?
	FragColor = texture(finalColorTexture, outUv);
}