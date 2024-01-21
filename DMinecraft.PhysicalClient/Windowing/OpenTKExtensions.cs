using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Windowing
{
    internal static class OpenTKExtensions
    {
        public static NativeWindowSettings Copy(this NativeWindowSettings windowSettings)
        {
            ArgumentNullException.ThrowIfNull(windowSettings);
            return new NativeWindowSettings()
            {
                SharedContext = windowSettings.SharedContext,
                Icon = windowSettings.Icon,
                IsEventDriven = windowSettings.IsEventDriven,
                API = windowSettings.API,
                Profile = windowSettings.Profile,
                Flags = windowSettings.Flags,
                AutoLoadBindings = windowSettings.AutoLoadBindings,
                APIVersion = windowSettings.APIVersion,
                CurrentMonitor = windowSettings.CurrentMonitor,
                Title = windowSettings.Title,
                StartFocused = windowSettings.StartFocused,
                StartVisible = windowSettings.StartVisible,
                WindowState = windowSettings.WindowState,
                WindowBorder = windowSettings.WindowBorder,
                Location = windowSettings.Location,
                ClientSize = windowSettings.ClientSize,
                MinimumClientSize = windowSettings.MinimumClientSize,
                MaximumClientSize = windowSettings.MaximumClientSize,
                //Size = windowSettings.Size,
                //MinimumSize = windowSettings.MinimumSize,
                //MaximumSize = windowSettings.MaximumSize,
                AspectRatio = windowSettings.AspectRatio,
                //IsFullscreen = windowSettings.IsFullscreen,
                NumberOfSamples = windowSettings.NumberOfSamples,
                StencilBits = windowSettings.StencilBits,
                DepthBits = windowSettings.DepthBits,
                RedBits = windowSettings.RedBits,
                GreenBits = windowSettings.GreenBits,
                BlueBits = windowSettings.BlueBits,
                AlphaBits = windowSettings.AlphaBits,
                SrgbCapable = windowSettings.SrgbCapable,
                TransparentFramebuffer = windowSettings.TransparentFramebuffer,
                Vsync = windowSettings.Vsync,
                AutoIconify = windowSettings.AutoIconify
            };
        }
    }
}
