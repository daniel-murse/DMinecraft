using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.UI.Widgets
{
    internal interface IPlaceable
    {
        public Vector4 Bounds { get; set; }

        public Vector2 Position { get; set; }

        public Vector2 Size { get; set; }
    }
}
