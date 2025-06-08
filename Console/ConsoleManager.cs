using TMPro;
using System.Collections.Generic;
using UnityEngine;
using PineappleMod.Tools;
using System;
using GorillaExtensions;
using System.Collections;

namespace PineappleMod.Console
{
    public class ConsoleManager : MonoBehaviour
    {

        public static ConsoleManager Instance { get; private set; }

        public GameObject console;
        public GameObject keyboard;


        public TextMeshPro consoleText;
        public TextMeshPro returnText;
        public List<GameObject> keys = new List<GameObject>();

        public GameObject backspace;
        public GameObject enter;
        public GameObject space;
        public GameObject shift;

        public Transform placeholder = new GameObject("AFHAUOHSGIOUHAG").transform;

        public bool shiftPressed = false;

        protected void Start()
        {
            Instance = this;
            Setup();
        }

        public void Setup()
        {
            console = Plugin.Instance.console.transform.Find("Screen").gameObject;
            consoleText = console.transform.Find("Input").GetComponent<TextMeshPro>();
            returnText = console.transform.Find("Output").GetComponent<TextMeshPro>();
            keyboard = Plugin.Instance.console.transform.Find("Keyboard").gameObject;

            if (!console || !consoleText || !keyboard)
            {
                return;
            }


            Logging.Info("ConsoleManager initialized, console and keyboard found."); // cHECK

            var renderer = console.GetComponent<MeshRenderer>();
            renderer.materials[0] = Plugin.Instance.pineappleBundle.LoadAsset<Material>("Black Material");
            renderer.materials[1] = Plugin.Instance.pineappleBundle.LoadAsset<Material>("m_Menu Outer");


            var keybaord = keyboard.GetComponent<MeshRenderer>();
            keybaord.material = Plugin.Instance.pineappleBundle.LoadAsset<Material>("m_Menu Outer");

            /*console.transform.SetParent(GorillaTagger.Instance.bodyCollider.transform, false);
            keyboard.transform.SetParent(GorillaTagger.Instance.bodyCollider.transform, false);

            console.transform.localPosition = new Vector3(0, 0.25f, 0.7782f);
            console.transform.localRotation = Quaternion.Euler(270, 270, 0);
            console.transform.localScale = new Vector3(1.895491f, 24.84053f, 13.98671f);

            keyboard.transform.localPosition = new Vector3(0, -0.05f, 0.5782f);
            keyboard.transform.localRotation = Quaternion.Euler(300.132111f, 180f, 90f);
            keyboard.transform.localScale = new Vector3(13.2667866f, 21.2343521f, 2.71945715f);*/

            placeholder.SetParent(GorillaTagger.Instance.bodyCollider.transform, false);
            placeholder.localPosition = new Vector3(0, 0, 0.7782f);

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
                    var k = child.AddComponent<Key>();
                    k.enabled = true;

                    var c = child.GetOrAddComponent<BoxCollider>();
                    c.isTrigger = true;
                    child.layer = 18;

                    switch (child.name)
                    {
                        case "Backspace": backspace = child; break;
                        case "Return": enter = child; break;
                        case "Shift": shift = child; break;
                        case "Space": space = child; break;
                        default:
                            Logging.Info($"Adding key: {child.name}");
                            k.OnPressed += OnKeyPressed;
                            keys.Add(child);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Logging.Fatal("Error init keyboard children / keys. ", e);
            }


            var spaceKey = space.GetComponent<Key>();
            spaceKey.OnPressed += OnKeyPressed;

            var backspaceKey = backspace.GetComponent<Key>();
            backspaceKey.OnPressed += OnBackspacePressed;

            var shiftKey = shift.GetComponent<Key>();
            shiftKey.OnPressed += OnShiftPressed;

            var enterKey = enter.GetComponent<Key>();
            enterKey.OnPressed += RunAndParseCommand;

            consoleText.text = "> ";
            returnText.text = "";
            Logging.Info("Success!", 96);
        }

        public void FixedUpdate()
        {
            if (console.activeSelf)
            {
                // Follow the position of the placeholder as before
                Plugin.Instance.console.transform.position = Vector3.Slerp(
                    Plugin.Instance.console.transform.position,
                    placeholder.position,
                    Time.deltaTime * 5
                );


                Plugin.Instance.console.transform.rotation = Quaternion.Slerp(
                     Plugin.Instance.console.transform.rotation,
                     GorillaTagger.Instance.mainCamera.transform.rotation * Quaternion.Euler(0, 270, 0),
                    Time.deltaTime * 1.5f
                );


            }
        }

        /// <summary>
        /// Handles the key press event for the console keys.
        /// </summary>
        /// <param name="key">The specific key pressed, this can be null</param>
        /// <param name="value">The value to add to the input</param>
        public void OnKeyPressed(Key key, string value)
        {
            value = shiftPressed ? value.ToUpper() : value.ToLower();

            consoleText.text += value;

            if (shiftPressed) OnShiftPressed();
        }
        /// <summary>
        /// Removes the latest character entry in the input. This is safe, it keeps the prompt ("> ") intact and only removes the last character of the input text. 
        /// </summary>
        /// <param name="key">Not used</param>
        /// <param name="value">Not used</param>
        public void OnBackspacePressed(Key key = null, string value = "")
        {
            if (consoleText.text.Length > 1)
            {
                consoleText.text = consoleText.text.Substring(0, consoleText.text.Length - 1);
            }
        }
        /// <summary>
        /// Toggles the shift state, changing the text case of the keys accordingly. You can use this in a script to automaticaly change case.
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="value"></param>
        public void OnShiftPressed(Key arg = null, string value = "")
        {
            shiftPressed = !shiftPressed;

            foreach (GameObject key in keys)
            {
                key.GetComponentInChildren<TextMeshPro>().text = shiftPressed ? key.name.ToUpper() : key.name.ToLower();
            }
        }

        public bool dontClearThisCommand = false;
        /// <summary>
        /// Runs the command and parses the input from the console. Value is not used, its a placeholder for key.cs actions, set the console text instead.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void RunAndParseCommand(Key key = null, string value = "")
        {
            // Only parse if there is actual input after the prompt
            var input = consoleText.text.TrimStart('>', ' ');
            if (string.IsNullOrWhiteSpace(input))
                return;

            try
            {
                Parser parser;
                if (Parser.Instance == null) parser = new Parser();
                else parser = Parser.Instance;
                var result = parser.ParseAndExecute(input);
                if (!result.Contains("$ "))
                {
                    returnText.text = $"Error: {result}";
                }
                else
                {
                    returnText.text = result;
                }
                Logging.Info($"Command executed: {input} - Result: {result}");
            }
            catch (Exception ex)
            {
                returnText.text = $"Error: {ex.Message}";
                Logging.Fatal("Parser error: ", ex);
            }

            consoleText.text = "> ";
            if (dontClearThisCommand)
            {
                dontClearThisCommand = false;
                return;
            }
            StartCoroutine(clearCache());
        }

        public IEnumerator clearCache()
        {
            if (Configuration.cacheClearTime.Value < 0)
            {
                returnText.text = "$ ";
                yield break;
            }
            yield return new WaitForSeconds(Configuration.cacheClearTime.Value);
            returnText.text = "$ ";
        }

        // Command Structure
        // <Namespace> [Command] {Args}
        // <Grate> [setmod] {fly enabled}
        // args[0..3]
    }
}