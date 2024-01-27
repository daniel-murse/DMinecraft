using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Scenes
{
    internal interface IScene
    {
        public void Update(TimeSpan deltaTime);

        public void Render(TimeSpan deltaTime);
    }
}
