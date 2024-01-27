using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects.Data.Programs
{
    internal class OutputResource
    {
        public OutputResource(int location, int type, int arraySize, string name)
        {
            Location = location;
            Type = type;
            ArraySize = arraySize;
            Name = name;
        }

        public int Location { get; init; }

        public int Type { get; init; }

        public int ArraySize { get; init; }

        public string Name { get; init; }
    }
}
