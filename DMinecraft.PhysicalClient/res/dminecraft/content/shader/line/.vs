#version 460 core

layout (location = 0) in vec3 aPos;

layout (location = 1) in vec4 aColor;  

layout (location = 0) uniform mat4 uTransform;

out vec4 vertexColor; // specify a color output to the fragment shader

void main()
{
    gl_Position = vec4(aPos, 1.0) * uTransform;
    vertexColor = aColor;
}