using System;
using System.IO;
using SWD.Utils;

namespace SWD
{
    public static class Settings
    {
        internal static string OS { get; }
        internal static string CurrentPath { get; }
        internal static int DefaultTimeout { get; set; }
        internal static WaitStrategy WaitStrategy { get; private set; }

        static Settings()
        {
            OS = Environment.OSVersion.ToString();
            CurrentPath = Directory.GetCurrentDirectory();
        }

        public static void Set(SettingsEnum setting, object value)
        {
            switch (setting)
            {
                case SettingsEnum.DefaultTimeout:
                    DefaultTimeout = (int) value;
                    break;
                case SettingsEnum.WaitStrategy:
                    WaitStrategy = (WaitStrategy) value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(setting), setting, null);
            }
        }
    }

    public enum SettingsEnum
    {
        DefaultTimeout,
        WaitStrategy,
    }
}