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

        public static void Init(ConfigFile File)
        {
            file = File;
            cacheClearTime = file.Bind("General", "Cache Clear Time", 2.5f, "The amount (in seconds) it will take for the console cache to clear. Set to -1 if you do not want the cache to clear.");
        }
    }
}
