using DMinecraft.PhysicalClient.Windowing;

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

        public AppSettings() 
        {
            WindowSettings = new AppWindowSettings();
            EnableHighPeriodTimer = true;
            IsDebug = true;
            EnableSleep = true;
            SleepError = TimeSpan.FromMilliseconds(1);
            UpdateFrequency = TimeSpan.FromMilliseconds(16);
            RenderFrequency = TimeSpan.FromMilliseconds(16);
        }

        public AppSettings(string[] args) : this()
        {
            WindowSettings = new AppWindowSettings();
        }

        public AppWindowSettings WindowSettings { get; init; }

        public bool EnableHighPeriodTimer { get; init; }

        public bool IsDebug { get; init; }

        public bool EnableSleep { get; init; }

        public TimeSpan SleepError { get; init; }

        public TimeSpan UpdateFrequency { get; init; }

        public TimeSpan RenderFrequency { get; init; }
    }
}