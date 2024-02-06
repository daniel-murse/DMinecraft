#version 460 core
out vec4 FragColor;
  
in vec4 vertexColor; // the input variable from the vertex shader (same name and same type)  
in vec2 uv;
in float layer;

const float smoothing = 1.0/16.0;

layout (location = 1) uniform sampler2DArray uAlbedo;

void main()
{
    float distance = texture(uAlbedo, vec3(uv, layer)).a;
    float alpha = smoothstep(0.5 - smoothing, 0.5 + smoothing, distance);
    FragColor = vec4(vertexColor.rgb, vertexColor.a * alpha);
} 