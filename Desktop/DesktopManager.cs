using PineappleMod.Console;
using PineappleMod.ConsoleCommands;
using PineappleMod.Tools;
using System;
using System.Linq;
using UnityEngine;

namespace PineappleMod.Desktop
{
    public class DesktopManager : MonoBehaviour
    {
        public static DesktopManager Instance { get; private set; }
        string input = "";
        string output = "";
        Rect windowRect = new Rect(100, 100, 300, 200);
        bool focusInput = false;

        protected void Start()
        {
            Instance = this;
        }

        void OnGUI()
        {
            windowRect = GUI.Window(0, windowRect, WindowEditor, "Pineapple Terminal Desktop", GUI.skin.window);
        }

        void WindowEditor(int id)
        {
            GUI.DragWindow(new Rect(1, 1, 5000, 5000));
            GUILayout.BeginVertical();
            GUILayout.Label("Pineapple Terminal Desktop");
            GUI.SetNextControlName("InputField");
            input = GUILayout.TextField(input);

            if (focusInput)
            {
                GUI.FocusControl("InputField");
                focusInput = false;
            }

            if (GUILayout.Button("Run"))
            {
                Parser parser = new Parser();
                Logging.Info(parser.ParseAndExecute(input));
                input = "";
                focusInput = true; // Set focus to input field after running
            }
            GUILayout.EndVertical();
        }
    }
}
