using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Text.Content
{
    internal class FileFontSource
    {
        public FileFontSource() 
        {
        
        }

        [JsonPropertyName("fontFile")]
        public string FontFile { get; init; }

        [JsonPropertyName("glyphs")]
        public string GlyphsPath { get; init; }
    }
}
