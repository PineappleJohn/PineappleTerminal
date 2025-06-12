using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace PineappleMod.Tools
{
    public static class Configuration
    {
        private static ConfigFile file;

        public static ConfigEntry<float> cacheClearTime;
        public static ConfigEntry<bool> pausePlayer;
        public static ConfigEntry<bool> openOnStart;

        public static void Init(ConfigFile File)
        {
            file = File;
            cacheClearTime = file.Bind("General", "Cache Clear Time", 2.5f, "The amount (in seconds) it will take for the console cache to clear. Set to -1 if you do not want the cache to clear.");
            pausePlayer = file.Bind("General", "Pause Player", true, "If true, the player will be paused when the console is open. If false, the player will not be paused and can move around freely while the console is open.");
            openOnStart = file.Bind("General", "Open On Start", true, "If true, the console will open automatically when the game starts.");
        }
    }
}
