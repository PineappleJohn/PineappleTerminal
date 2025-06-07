using PineappleMod.Console;
using PineappleMod.Tools;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace PineappleMod.Menu
{
    public class MenuManager : MonoBehaviour
    {
        public static MenuManager instance;

        protected void Start()
        {
            instance = this;
            Setup();
        }

        public void Setup()
        {
            GestureTracker.Instance.rightGrip.OnReleased += ToggleMenu;
        }

        public void ToggleMenu(InputTracker t)
        {
            if (ConsoleManager.Instance.console.activeSelf)
            {
                DisableMenu();
            }
            else
            {
                EnableMenu();
            }
        }

        public void EnableMenu()
        {
            ConsoleManager.Instance.console.SetActive(true);
            ConsoleManager.Instance.keyboard.SetActive(true);
        }

        public void DisableMenu()
        {
            ConsoleManager.Instance.console.SetActive(false);
            ConsoleManager.Instance.keyboard.SetActive(false);
        }
    }
}