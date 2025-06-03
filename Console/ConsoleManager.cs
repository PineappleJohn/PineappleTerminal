using TMPro;
using System.Collections.Generic;
using UnityEngine;
using PineappleMod.Tools;
using System;
using GorillaNetworking;

namespace PineappleMod.Console
{
    public class ConsoleManager : MonoBehaviour
    {
        public static ConsoleManager Instance { get; private set; }

        public GameObject console;
        public GameObject keyboard;


        public TextMeshPro consoleText;
        public List<GameObject> keys = new List<GameObject>();

        public GameObject backspace;
        public GameObject enter;
        public GameObject space;
        public GameObject shift;

        public bool shiftPressed = false;

        protected void Start() {
            Instance = this;
            Setup();
        }

        public void Setup() {
            console = Plugin.Instance.console.transform.Find("Screen").gameObject;
            consoleText = console.transform.Find("Input").GetComponent<TextMeshPro>();
            keyboard = Plugin.Instance.console.transform.Find("Keyboard").gameObject;

            if (!console || !consoleText || !keyboard) {
                ErrorOccur(new object[] { console, consoleText, keyboard, 37 });
                return;
            }


            Logging.Info("ConsoleManager initialized, console and keyboard found."); // cHECK

            var renderer = console.GetComponent<MeshRenderer>();
            renderer.materials[0] = Plugin.Instance.pineappleBundle.LoadAsset<Material>("Black Material");
            renderer.materials[1] = Plugin.Instance.pineappleBundle.LoadAsset<Material>("m_Menu Outer");


            var keybaord = keyboard.GetComponent<MeshRenderer>();
            keybaord.material = Plugin.Instance.pineappleBundle.LoadAsset<Material>("m_Menu Outer");

            console.transform.SetParent(GorillaTagger.Instance.bodyCollider.transform, false);
            keyboard.transform.SetParent(GorillaTagger.Instance.bodyCollider.transform, false);

            Logging.Info(console, consoleText, keyboard);

            console.transform.localPosition = new Vector3(0, 0.25f, 0.7782f);
            console.transform.localRotation = Quaternion.Euler(270, 270, 0);
            console.transform.localScale = new Vector3(1.895491f, 24.84053f, 13.98671f);

            keyboard.transform.localPosition = new Vector3(0, -0.05f, 0.5782f);
            keyboard.transform.localRotation = Quaternion.Euler(300.132111f, 180f, 90f);
            keyboard.transform.localScale = new Vector3(13.2667866f, 21.2343521f, 2.71945715f);

            Logging.Info($"keyboard has {keyboard.transform.childCount} direct children");
            for (int i = 0; i < keyboard.transform.childCount; i++)
            {
                Logging.Info($"Direct child {i}: {keyboard.transform.GetChild(i).name}");
            }

            try
            {
                for (int i = 0; i < keyboard.transform.childCount; i++)
                {
                    var child = keyboard.transform.GetChild(i).gameObject;
                    child.layer = LayerMask.NameToLayer("GorillaInteractible");

                    Logging.Info($"Keyboard child: {i} : {child.name}");

                    switch (child.name)
                    {
                        case "Backspace": backspace = child.gameObject; break;
                        case "Return": enter = child.gameObject; break;
                        case "Shift": shift = child.gameObject; break;
                        case "Space": space = child.gameObject; break;
                        default:
                            var k = child.AddComponent<Key>();
                            k.characterString = shiftPressed ? child.name.ToUpper() : child.name.ToLower();
                            k.OnButtonPressedEvent += OnKeyPressed;
                            keys.Add(child.gameObject);
                            break;
                    }
                }
            }
            catch (Exception e) {
                Logging.Fatal("Error init keyboard children / keys. ", e);
            }


            var spaceKey = space.AddComponent<Key>();
            spaceKey.characterString = " ";
            spaceKey.OnButtonPressedEvent += OnKeyPressed;

            var backspaceKey = backspace.AddComponent<Key>();
            backspaceKey.OnButtonPressedEvent += OnBackspacePressed;

            var shiftKey = shift.AddComponent<Key>(); 
            shiftKey.OnButtonPressedEvent += OnShiftPressed;

            var enterKey = enter.AddComponent<Key>();
            enterKey.OnButtonPressedEvent += RunAndParseCommand;

            consoleText.text = "> ";
            Logging.Info("Success!", 96);
        }

        public void ErrorOccur(object[] args) {
            Logging.Fatal($"An error occured with ConsoleManager, more details {args}");
        }

        public void OnKeyPressed(string value) {
            if (value.IsNullOrEmpty()) return;

            value = shiftPressed ? value.ToUpper() : value.ToLower();

            consoleText.text += value;

            if (shiftPressed) OnShiftPressed();
        }

        public void OnBackspacePressed(string value = "")
        {
            if (consoleText.text.Length > 1)
            {
                consoleText.text = consoleText.text.Substring(0, consoleText.text.Length - 1);
            }
        }

        public void OnShiftPressed(string value = "")
        {
            shiftPressed = !shiftPressed;

            foreach (GameObject key in keys) {
                key.GetComponentInChildren<TextMeshPro>().text = shiftPressed ? key.name.ToUpper() : key.name.ToLower();
            }
        }

        public void RunAndParseCommand(string value = "") {
            if (consoleText.text.IsNullOrEmpty()) return;
            consoleText.text = "> ";
            //Logic here
        }
    }
}
