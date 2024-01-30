#version 460 core
out vec4 FragColor;
  
in vec4 vertexColor; // the input variable from the vertex shader (same name and same type)  
in vec2 uv;
in float layer;

layout (location = 1) uniform sampler2DArray uAlbedo;

void main()
{

    FragColor = (vertexColor * texture(uAlbedo, vec3(uv, layer)));
} 