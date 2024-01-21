using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Windowing
{
    internal class AppWindow : NativeWindow
    {
        public AppWindow(AppWindowSettings settings) : base(settings.NativeWindowSettings)
        {

        }

        public void ProcessEvents()
        {
            ProcessEvents(0);
        }

        
    }
}
