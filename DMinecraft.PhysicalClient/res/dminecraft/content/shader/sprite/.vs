#version 460 core

layout (location = 0) in vec3 aPos;

layout (location = 1) in uint aColor;  

layout (location = 2) in vec2 aUv;

layout(location = 3) in ivec2 aLayerIndex;

layout (location = 0) uniform mat4 uTransform;

out vec4 vertexColor; // specify a color output to the fragment shader
out vec2 uv;
out float layer;

void main()
{
    gl_Position = vec4(aPos, 1.0) * uTransform;
    //vertexColor = unpackUnorm4x8(aColor);
    //order of components
    //https://registry.khronos.org/OpenGL-Refpages/gl4/html/unpackUnorm.xhtml
    vertexColor = unpackUnorm4x8(aColor).abgr;
    uv = aUv;
    layer = aLayerIndex.x;
}