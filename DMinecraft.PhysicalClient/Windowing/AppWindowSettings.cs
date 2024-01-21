using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Windowing
{
    internal class AppWindowSettings
    {
        public AppWindowSettings()
        {
            NativeWindowSettings = new NativeWindowSettings();
        }

        public AppWindowSettings(AppWindowSettings windowSettings)
        {
            ArgumentNullException.ThrowIfNull(windowSettings, nameof(windowSettings));
            NativeWindowSettings = windowSettings.NativeWindowSettings.Copy();
        }

        public NativeWindowSettings NativeWindowSettings { get; }
    }
}
