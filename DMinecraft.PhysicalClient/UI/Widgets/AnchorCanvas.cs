using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.UI.Widgets
{
    internal class AnchorCanvas
    {
        public Vector4 Bounds { get; private set; }

        public Vector2 Size { get; private set; }

        public Vector2 Position { get; private set; }

        public void AddWidget(IPlaceable widget, Vector4 anchors, Vector4 offsets)
        {
            //anchors is left bottom right top
            
        }

        public Vector4 ComputeBounds(Vector4 anchors, Vector4 offsets)
        {
            return new Vector4(
                offsets.X + (anchors.X * Size.X) + Position.X,
                offsets.Y + (anchors.Y * Size.Y) + Position.Y,
                -offsets.Z + ((1 - anchors.Z) * Size.X) + Position.X,
                -offsets.W + ((1 - anchors.W) * Size.Y) + Position.Y);
        }
    }
}
