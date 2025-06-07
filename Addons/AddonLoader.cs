using BepInEx;
using PineappleMod.Console.Command;
using PineappleMod.ConsoleCommands;
using PineappleMod.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PineappleMod.Addons
{
    public static class AddonLoader
    {
        public static void LoadAddons() { 
            string addonPath = Path.Combine(Paths.PluginPath, "Pineapples");
            if (Directory.Exists(addonPath))
            {
                foreach (string file in Directory.GetFiles(addonPath, "*.dll"))
                {
                    try
                    {
                        Logging.Info($"Loading addon: {file}");
                        var assembly = Assembly.LoadFrom(file);
                        var types = assembly.GetTypes()
                            .Where(t => typeof(Namespace).IsAssignableFrom(t) && !t.IsAbstract);

                        foreach (var type in types)
                        {
                            if (Activator.CreateInstance(type) is Namespace pineapple)
                            {
                                CommandList.Namespaces.Add(pineapple.GetNamespaceName(), pineapple);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Logging.Fatal($"Failed to load addon {file}: {e.Message}");
                    }
                }
            }
            else
            {
                Logging.Warning("Pineapples path not found.");
            }
        }
    }
}
