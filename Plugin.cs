using BepInEx;
using UnityEngine;
using PineappleMod.Tools;
using System;
using PineappleMod.Menu;
using PineappleMod.Console;
using PineappleMod.Desktop;

namespace PineappleMod
{
    [BepInIncompatibility("homeoverspend.monosandbox")]
    [BepInIncompatibility("org.iidk.gorillatag.iimenu")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance { get; private set; }
        public AssetBundle pineappleBundle;
        public GameObject pineappleObj;
        public GameObject console;

        public GameObject m_Menu, m_Console, m_Gesture;

        protected void Awake()
        {
            Instance = this;
            GorillaTagger.OnPlayerSpawned(OnGameInit);
        }

        public void OnEnable() {
            GestureTracker.Instance.rightGrip.OnReleased += MenuManager.instance.ToggleMenu;
        }

        public void OnDisable() {
            GestureTracker.Instance.rightGrip.OnReleased -= MenuManager.instance.ToggleMenu;
            MenuManager.instance.DisableMenu();
        }


        public void OnGameInit() // Lots of try/catch blocks so i can see if things init properly, might remove later
        {
            Configuration.Init(Config);
            Logging.Init();
            Logging.Info("PineappleMod is starting up...");
            Logging.Info("Attempting to create console and menu objects...");
            try
            {
                m_Console = new GameObject("PineappleConsoleManager");
                m_Console.AddComponent<Console.ConsoleManager>();

                m_Menu = new GameObject("PineappleMenuManager"); // These namespaces are satisfying
                m_Menu.AddComponent<Menu.MenuManager>();

                m_Gesture = new GameObject("PineappleGestureTracker");
                m_Gesture.AddComponent<GestureTracker>();

#if DEBUG
                new GameObject("PineappleDebugManager").AddComponent<DesktopManager>();
#endif

                Logging.Info("Console and menu objects created / init success");
            }
            catch (Exception e)
            {
                Logging.Fatal("Failed to create console and menu objects! ", e);
                Logging.Fatal("More details ", m_Console.ToString(), m_Menu.ToString(), m_Gesture.ToString());
            }
            Logging.Info("Attempting to create asset bundle!");
            try
            {
                pineappleBundle = Tools.AssetUtils.LoadAssetBundle("PineappleMod/Resources/pineapplemod");
                console = Instantiate(pineappleBundle.LoadAsset<GameObject>("PineappleConsole"));
                console.name = "PineappleConsole";
                Logging.Info("Succesfully loaded asset bundle!");
            }
            catch (Exception e)
            {
                Logging.Fatal("Failed to load asset bundle! ", e);
            }
        }
        /// <summary>
        /// DEPRECATED, DO NOT USE
        /// </summary>
        /// <returns></returns>
        public Vector3 getConsoleScale() {
            return Vector3.one * 0.05406467f;
        }
    }



    public class PluginInfo
    {
        internal const string
            GUID = "John.PineappleMod",
            Name = "Pineapple Terminal",
            Version = "1.0.0";
    }
}
