using OpenTK.Windowing.Common;
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
            NativeWindowSettings.APIVersion = new Version(4, 6);
            NativeWindowSettings.Flags = ContextFlags.Debug | ContextFlags.ForwardCompatible;
        }

        public AppWindowSettings(AppWindowSettings windowSettings)
        {
            ArgumentNullException.ThrowIfNull(windowSettings, nameof(windowSettings));
            NativeWindowSettings = windowSettings.NativeWindowSettings.Copy();
        }

        public NativeWindowSettings NativeWindowSettings { get; }
    }
}
