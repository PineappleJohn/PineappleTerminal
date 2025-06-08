using GorillaLocomotion;
using PineappleMod.Console;
using PineappleMod.Tools;
using UnityEngine;

namespace PineappleMod.Menu
{
    public class MenuManager : MonoBehaviour
    {
        public static MenuManager instance;
        Vector3 savedVelocity;
        bool menuon = false;

        protected void Start()
        {
            instance = this;
            Setup();
        }

        public void Setup()
        {
            Destroy(Plugin.Instance.console.transform.Find("Menu").gameObject);
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

        public void Update()
        {
            if (menuon)
                GorillaTagger.Instance.rigidbody.AddForce(-UnityEngine.Physics.gravity * GorillaTagger.Instance.rigidbody.mass * GTPlayer.Instance.scale);
        }

        public void EnableMenu()
        {
            menuon = true;
            savedVelocity = GorillaTagger.Instance.rigidbody.velocity;
            GorillaTagger.Instance.rigidbody.velocity = Vector3.zero;
            ConsoleManager.Instance.console.SetActive(true);
            ConsoleManager.Instance.keyboard.SetActive(true);
        }

        public void DisableMenu()
        {
            menuon = false;
            ConsoleManager.Instance.console.SetActive(false);
            ConsoleManager.Instance.keyboard.SetActive(false);
            GorillaTagger.Instance.rigidbody.AddForce(savedVelocity);
        }
    }
}