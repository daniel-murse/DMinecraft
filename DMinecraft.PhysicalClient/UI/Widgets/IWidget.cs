using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.UI.Widgets
{
    internal interface IWidget : IPlaceable
    {
        public void Render();

        public void Update();
    }
}
