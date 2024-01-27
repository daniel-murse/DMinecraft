using DMinecraft.PhysicalClient.Windowing;
using System.Diagnostics.CodeAnalysis;

namespace DMinecraft.PhysicalClient
{
    internal class AppSettings
    {
        public AppSettings(AppSettings appSettings)
        {
            ArgumentNullException.ThrowIfNull(appSettings);

            WindowSettings = new AppWindowSettings(appSettings.WindowSettings);
            EnableHighPeriodTimer = appSettings.EnableHighPeriodTimer;
            IsDebug = appSettings.IsDebug;
            EnableSleep = appSettings.EnableSleep;
            SleepError = appSettings.SleepError;
            UpdateFrequency =  appSettings.UpdateFrequency;
            RenderFrequency = appSettings.RenderFrequency;
        }

        [SetsRequiredMembers]
        public AppSettings() 
        {
            WindowSettings = new AppWindowSettings();
            EnableHighPeriodTimer = true;
            IsDebug = true;
            EnableSleep = true;
            SleepError = TimeSpan.FromMilliseconds(1);
            UpdateFrequency = TimeSpan.FromMilliseconds(16);
            RenderFrequency = TimeSpan.FromMilliseconds(16);
            ContentRootPath = Directory.GetCurrentDirectory();
        }

        [SetsRequiredMembers]
        public AppSettings(string[] args) : this()
        {
            WindowSettings = new AppWindowSettings();
        }

        public required AppWindowSettings WindowSettings { get; init; }

        public bool EnableHighPeriodTimer { get; init; }

        public bool IsDebug { get; init; }

        public bool EnableSleep { get; init; }

        public TimeSpan SleepError { get; init; }

        public TimeSpan UpdateFrequency { get; init; }

        public TimeSpan RenderFrequency { get; init; }

        public required string ContentRootPath { get; init; }
    }
}