using PineappleMod.Console;
using PineappleMod.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PineappleMod.Commands._3rdParty
{
    public class Pineapples : MonoBehaviour
    {
        public static Pineapples Instance { get; private set; }
        public void Awake() {
            Instance = this;
            Invoke(nameof(DelayedPluginLoader), 0.1f);
        }

        public void DelayedPluginLoader() {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    if (assembly.FullName.Contains("PineappleMod")) continue;
                    Logging.Info($"Checking assembly: {assembly.FullName}");
                    var addonTypes = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Command)));
                    foreach (var addonType in addonTypes)
                    {
                        Logging.Info($"Found command: {addonType.FullName}");
                        // Attach to the same GameObject as Pineapples
                        Logging.Info($"Attempting to instantiate: {addonType.FullName}");
                        var command = (Command)Activator.CreateInstance(addonType);
                        Parser.Instance.RegisterCommand(command);

                    }
                }
                catch (Exception e)
                {
                    Logging.Fatal(Parser.Instance, assembly);
                    Logging.Fatal($"Error loading commands from assembly {assembly.FullName}: {e.Message}");
                }
            }
        }
    }
}
